using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.InteropServices.ComTypes;


// i have literally no idea what the fuck i am doing.
namespace MemPatchTest2
{
    class Program
    {
        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);
       
  


            public static void Main()
        {
            // + (ulong) 0x00D56000
            //0x00012875

            Process process = Process.GetProcessesByName("redacted")[0];
            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);
            IntPtr baseaddr = process.MainModule.BaseAddress;
            

            int bytesRead = 0;
            int bytesRead2 = 0;
            byte[] buffer = new byte[7];
            byte[] buffer3 = new byte[16];

            ReadProcessMemory((int)processHandle, (int)((ulong)baseaddr + (ulong)0x00012875), buffer, buffer.Length, ref bytesRead);
            ReadProcessMemory((int)processHandle, (int)((ulong)baseaddr + (ulong)0x000128A2), buffer3, buffer3.Length, ref bytesRead2);
            string s = "";
            for (int i = 0; i < buffer.Length; i++)
                s = s + buffer[i].ToString("X2") + " ";


            string s3 = "";
            for (int A = 0; A < buffer3.Length; A++)
                s3 = s3 + buffer3[A].ToString("X2") + " ";
            Console.WriteLine(String.Join(" ", "Found Bytes:" + s + "at 0x00012875" ));
            Console.WriteLine(String.Join(" ", "Found Bytes:" + s3 + "at 0x000128A2"));
            Console.WriteLine("(Press Enter)");
          //  File.AppendAllText(@"test.txt", bytesRead.ToString());
            Console.ReadLine();
            Console.WriteLine("Trying to patch");
            Console.ReadLine();
            int bytesWritten = 0;
            byte[] buffer2 = { 0x00 };
            byte[] buffer4 = { 0x75 };
            WriteProcessMemory((int)processHandle, (int)((ulong)baseaddr + (ulong)0x0001287b), buffer2, buffer2.Length, ref bytesWritten);
            WriteProcessMemory((int)processHandle, (int)((ulong)baseaddr + (ulong)0x000128B0), buffer4, buffer4.Length, ref bytesWritten);
            Console.WriteLine("success! ");
            Console.ReadLine();
            //2nd offset = 128CD 74 > 75
           // ReadProcessMemory((int)processHandle, (int)((ulong)baseaddr + (ulong)0x00012875), buffer2, buffer2.Length, ref bytesRead);
          
        
        }

    
    }

    }

