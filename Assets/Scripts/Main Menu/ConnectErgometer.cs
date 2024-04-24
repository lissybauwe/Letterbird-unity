using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

//this Code was taken from the Project ExertyFish
public class ConnectErgometer : MonoBehaviour
{
    private int i;
    public int bikeRes = 0; //variable to set in other class to change Resistance
    public static bool opencomportDone = false;
    public bool connected = false;
    private static bool occupied = false;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        opencomportDone = false;
    }

    private void FixedUpdate()
    {
        if (opencomportDone)
        {
            connected = true;
            if (i == 10) // request RPM at 0.5s
            {
                RequestRPM457();
                //UnityEngine.Debug.Log("RequestRPM");
            }

            if (i == 20) // read RPM at 0.7s
            {
                int temp_rpm = ReadRPM457();
              if(bikeRes == temp_rpm||temp_rpm == 0)
                {
                    UnityEngine.Debug.Log("RPM wrong");
                }
                else {
                    rpm = temp_rpm;
                    UnityEngine.Debug.Log("RPM: " + rpm);
                }
                
            }

            if (i == 35) // request HR at 1s
            {
                RequestHR457();
                //UnityEngine.Debug.Log("RequestHR");
            }

            if (i == 45) // read HR at 1.2s
            {
                hr = ReadHR457();
                //UnityEngine.Debug.Log("HR: " + hr);
            }

            if (i == 70) // setRes at 1.5s
            {
                //UnityEngine.Debug.Log("set bikeRes: " + bikeRes);
                SetRes(bikeRes);
            }

            if (i == 80) // reset i
            {
                i = 0;
                //UnityEngine.Debug.Log("Reset");
            }


            i++;
        }
        else
        {
            connected = false;
        }
    }

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
        occupied = true;
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
                    occupied = false;
                    return rpm;
                }
                occupied = false;
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
        occupied = false;
        return rpm;
    }

    static int isReadingRPM = 0;
    static int comPortNumber = 4; //edit this number to the fitting port number u put the usb cabel in!
    static IntPtr comPort;
    static byte address = 0;
    public int hr = 0; //variable to read in other class to access HeartRate
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

        if (occupied) { return resistance; }

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
            opencomportDone = false;
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
        if (!occupied) {

            while (isReadingRPM != 0)
            {
                Thread.Sleep(1);
            }

            bikeResistance = SetResistance457(bikeResistance);
        }
    }

    static int lastRpm;
    public static void Main()
    {
        Thread threadComPort = new Thread(OpenComPort2);
        threadComPort.Start();

    }
}
