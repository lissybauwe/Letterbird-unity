using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Threading;
using System;
using System.Diagnostics;
// Debug Command for Console: UnityEngine.Debug.Log("Text shows in Console");

public class ErgometerScript : MonoBehaviour
{

    private int i;
    public int bikeRes = 30; //variable to set in other class to change Resistance
    private static bool opencomportDone = false;

    private void Start()
    {
        opencomportDone = true;
    }

    private void Update()
    {
        if (opencomportDone)
        {
            if (i == 40) // request RPM at 0.5s
            {
                RequestRPM457();
            }

            if (i == 70) // read RPM at 0.7s
            {
                int temp_rpm = ReadRPM457();
                if(temp_rpm != 0)
                {
                    if(temp_rpm > rpm - 10 || temp_rpm < rpm + 10)
                    {
                        if(temp_rpm != bikeRes)
                        {
                            rpm = temp_rpm;
                        }
                    }
                }
            }

            if (i == 110) // request HR at 1s
            {
                RequestHR457();
            }

            if (i == 140) // read HR at 1.2s
            {
                hr = ReadHR457();
            }

            if (i == 170) // setRes at 1.5s
            {
                UnityEngine.Debug.Log("HR: " + hr + ", RPM: " + rpm);
                SetRes(bikeRes);
            }

            if (i == 200) // reset i
            {
                i = 0;
            }


            i++;
        }


    }

    static int isReadingRPM = 0;
    static int comPortNumber = 4; //edit this number to the fitting port number u put the usb cabel in!
    static IntPtr comPort;
    static byte address = 0;
    public int hr = 90; //variable to read in other class to access HeartRate
    public int rpm = 0; //variable to read in other class to access RPM

    [DllImport("kernel32.dll")]
    static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    [DllImport("kernel32.dll")]
    static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

    [DllImport("kernel32.dll")]
    static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

    [StructLayout(LayoutKind.Sequential)]
    struct DCB
    {
        public uint DCBlength;
        public uint BaudRate;
        public uint Flags;
        public ushort wReserved;
        public ushort XonLim;
        public ushort XoffLim;
        public byte ByteSize;
        public byte Parity;
        public byte StopBits;
        public sbyte XonChar;
        public sbyte XoffChar;
        public sbyte ErrorChar;
        public sbyte EofChar;
        public sbyte EvtChar;
        public ushort wReserved1;
        public int fRtsControl;
    }

    [DllImport("kernel32.dll")]
    static extern bool GetCommState(IntPtr hFile, ref DCB lpDCB);

    [DllImport("kernel32.dll")]
    static extern bool SetCommState(IntPtr hFile, ref DCB lpDCB);

    [StructLayout(LayoutKind.Sequential)]
    struct COMMTIMEOUTS
    {
        public uint ReadIntervalTimeout;
        public uint ReadTotalTimeoutMultiplier;
        public uint ReadTotalTimeoutConstant;
        public uint WriteTotalTimeoutMultiplier;
        public uint WriteTotalTimeoutConstant;
    }

    [DllImport("kernel32.dll")]
    static extern bool SetCommTimeouts(IntPtr hFile, ref COMMTIMEOUTS lpCommTimeouts);

    static void RequestHR457()
    {
        byte[] buffer = { 0x01, 0x50, 0x03, 0x52, 0x17 };
        uint written = 0;
        WriteFile(comPort, buffer, 5, out written, IntPtr.Zero);
    }

    public static int ReadHR457()
    {
        int hr = 0;
        for (int i = 0; i < 100; ++i)
        {
            byte[] buffer = new byte[1];
            uint numBytesRead = 0;
            ReadFile(comPort, buffer, 1, out numBytesRead, IntPtr.Zero);
            byte b = buffer[0];
            if (numBytesRead == 0)
            {
                if (0 <= hr && hr <= 205)
                {
                    return hr;
                }
                return 0;
            }
            else if (i == 3)
            {
                hr += 100 * ((char)b - '0');
            }
            else if (i == 4)
            {
                hr += 10 * ((char)b - '0');
            }
            else if (i == 5)
            {
                hr += ((char)b - '0');
            }
        }
        return hr;
    }



    static void RequestRPM457()
    {
        byte[] buffer = { 0x01, 0x44, 0x03, 0x46, 0x17 };
        uint written = 0;
        WriteFile(comPort, buffer, 5, out written, IntPtr.Zero);
    }

    public static int ReadRPM457()
    {
        int rpm = 0;
        for (int i = 0; i < 100; ++i)
        {
            byte[] buffer = new byte[1];
            uint numBytesRead = 0;
            ReadFile(comPort, buffer, 1, out numBytesRead, IntPtr.Zero);
            byte b = buffer[0];
            if (numBytesRead == 0)
            {
                if (0 <= rpm && rpm <= 150)
                {
                    return rpm;
                }
                return 0;
            }
            else if (i == 3)
            {
                rpm += 100 * ((char)b - '0');
            }
            else if (i == 4)
            {
                rpm += 10 * ((char)b - '0');
            }
            else if (i == 5)
            {
                rpm += ((char)b - '0');
            }
        }
        return rpm;
    }

    static byte ComputeChecksum457(byte[] buffer)
    {
        int size = buffer.Length;
        byte checksum = 0x00;
        for (int i = 0; i < size; i++)
        {
            checksum ^= buffer[i];
            if (buffer[i] == 0x03)
                break;
        }
        return checksum;
    }

    static int SetResistance457(int resistance)
    {
        if (resistance < 0)
            resistance = 0;
        if (resistance > 400)
            resistance = 400;

        char[] resChars = resistance.ToString("D3").ToCharArray();

        isReadingRPM = 1;
        byte[] buffer = { 0x01, 0x57, 0x02, (byte)resChars[0], (byte)resChars[1], (byte)resChars[2], 0x03, 0x00, 0x17 };
        byte checksum = ComputeChecksum457(buffer);
        buffer[7] = checksum;

        uint written = 0;
        WriteFile(comPort, buffer, 9, out written, IntPtr.Zero);
        Thread.Sleep(50);

        byte[] answer = new byte[9];
        uint numBytesRead = 0;
        ReadFile(comPort, answer, 9, out numBytesRead, IntPtr.Zero);
        isReadingRPM = 0;
        if (numBytesRead != 9)
        {
            return -(int)numBytesRead;
        }

        int answerResistance = 100 * (int)(answer[3] - '0') + 10 * (int)(answer[4] - '0') + (int)(answer[5] - '0');
        return answerResistance;
    }

    static void OpenComPort2(object data)
    {
        COMMTIMEOUTS timeouts;
        string comName = "\\\\.\\COM" + comPortNumber;


        //UnityEngine.Debug.Log("Test");

        comPort = CreateFile(comName, 0x80000000 | 0x40000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);

        if (comPort == IntPtr.Zero)
        {

            UnityEngine.Debug.Log("IntPtrZero");
            return;

        }

        DCB deviceControlBlock = new DCB();
        deviceControlBlock.DCBlength = (uint)Marshal.SizeOf(typeof(DCB));

        if (GetCommState(comPort, ref deviceControlBlock) == false)
        {
            UnityEngine.Debug.Log("deviceControlBlock");
            return;
        }

        deviceControlBlock.BaudRate = 9600;
        deviceControlBlock.StopBits = 0;
        deviceControlBlock.Parity = 0;
        deviceControlBlock.ByteSize = 8;
        deviceControlBlock.fRtsControl = 0;
        if (SetCommState(comPort, ref deviceControlBlock) == false)
        {

            //UnityEngine.Debug.Log("setCommState");
            return;
        }

        timeouts.ReadIntervalTimeout = 50;
        timeouts.ReadTotalTimeoutMultiplier = 1;
        timeouts.ReadTotalTimeoutConstant = 1;
        timeouts.WriteTotalTimeoutMultiplier = 1;
        timeouts.WriteTotalTimeoutConstant = 1;
        if (SetCommTimeouts(comPort, ref timeouts) == false)
        {
            return;
        }

        //UnityEngine.Debug.Log("openComportDone");

        opencomportDone = true;

    }

    static void SetRes(int bikeResistance)
    {
        while (isReadingRPM != 0)
        {
            Thread.Sleep(1);
        }

        bikeResistance = SetResistance457(bikeResistance);
    }

    static int lastRpm;
    static void Main()
    {
        Thread threadComPort = new Thread(OpenComPort2);
        threadComPort.Start();

    }
}