using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using POS58Listen.PrintService;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Drawing.Printing;
using System.Drawing;

namespace POS58Listen
{
    class Program
    {
        public static int Asc(string character)
        {
            if (character.Length == 1)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
                return (intAsciiCode);
            }
            else
            {
                throw new Exception("Character is not valid.");
            }

        }

        static int x = 10;
        static int y = 0;
        public static void AddLine(ref Graphics g,string sMsg)
        {
            y += 20;
            g.DrawString(sMsg, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, x, y);
        }

        private static void printPageEvent(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string sOrderNo = string.Empty;
            string sItemName = string.Empty;
            string sItemQty = string.Empty;
            string sPrice = string.Empty;
            string sTotal = string.Empty;
            string iTotal = string.Empty;
            string sEname = string.Empty;
            string sDesc = string.Empty;
            PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            Graphics g = e.Graphics;
            SolidBrush pb = new SolidBrush(Color.Black);
            Single left = pd.DefaultPageSettings.Margins.Left - 10;
            Single top = pd.DefaultPageSettings.Margins.Top - 20;

            DataTable dist_dt = dt.DefaultView.ToTable(true, new string[] { "EName","OrderNo","Total","ODESC" });
            foreach (DataRow dr in dist_dt.Rows)
            {
                sOrderNo = dr["OrderNo"].ToString();
                sTotal = dr["Total"].ToString();
                sEname = dr["EName"].ToString();
                sDesc = dr["ODESC"].ToString();
                DataRow[] result = dt.Select("OrderNo='" + sOrderNo + "'");
                AddLine(ref g, "------------------------");
                AddLine(ref g, "訂購人: " + sEname);
                AddLine(ref g, "訂單編號: " + sOrderNo);
                AddLine(ref g, "訂單金額: " + sTotal);
                AddLine(ref g, "訂單明細:");
                foreach (DataRow dr2 in result)
                {

                    sItemName = dr2["Item"].ToString();
                    sItemQty = dr2["ItemQty"].ToString();
                    sPrice = dr2["DPrice"].ToString();
                    iTotal = dr2["iTotal"].ToString();
                    AddLine(ref g, sItemName + "    " + sPrice + " x " + sItemQty + " = " + iTotal);

                }
                AddLine(ref g, "備註: "+sDesc);
                Console.WriteLine("PrintReceptSoapClient start");
                POS58Listen.PrintService.OrderDataSoapClient pr = new POS58Listen.PrintService.OrderDataSoapClient();
                Console.WriteLine("PrintReceptSoapClient end");
                pr.UpdatePrintFlag(sOrderNo);
            }
            
            Console.WriteLine("Finish this stage");

        }

        static DataTable dt;
        static void Main(string[] args)
        {
            while (true)
            {
                StringBuilder sb = new StringBuilder();
                Console.WriteLine("Start Sleep 10 seconds");
                Thread.Sleep(10000);
                
                try
                {
                    Console.WriteLine("PrintReceptSoapClient start");
                    POS58Listen.PrintService.OrderDataSoapClient pr = new POS58Listen.PrintService.OrderDataSoapClient();
                    Console.WriteLine("PrintReceptSoapClient end");
                    DataSet xx = pr.getPrintOrderData();
                    dt = xx.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                        pd.PrintPage += printPageEvent;
                        pd.Print();
                        y = 0;
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }
            }
        }

        private static string PrinterName = "XP-58";
        private static RawPrinterHelper prn = new RawPrinterHelper();

        public static void StartPrint()
        {
            prn.OpenPrint(PrinterName);
        }

        public static void EndPrint()
        {
            prn.ClosePrint();
        }

        public static void Print(string Line)
        {
            prn.SendStringToPrinter(PrinterName, Line + "\r\n");
            
        }

        public static string eClear = (char)27 + "@";
        public static string eCentre = (char)(27) + (char)(97) + "1";
        public static string eLeft = (char)(27) + (char)(97) + "0";
        public static string eRight = (char)(27) + (char)(97) + "2";
        public static string eDrawer = eClear + (char)(27) + "p" + (char)(0) + ".}";
        public static string eCut = (char)(27) + "i" + "\r\n";
        public static string eSmlText = (char)(27) + "!" + (char)(1);
        public static string eNmlText = (char)(27) + "!" + (char)(0);
        public static string eInit = eNmlText + (char)(13) + (char)(27) + "c6" + (char)(1) + (char)(27) + "R3" + "\r\n";
        public static string eBigCharOn = (char)(27) + "!" + (char)(56);

        public static string eBigCharOff = (char)(27) + "!" + (char)(0);
        public static void PrintFooter()
        {
            Print(eCentre + "Thank You For Your Support!" + eLeft);
            Print("\r\n" + "\r\n" + eCut + eDrawer);
        }
    }

    public class RawPrinterHelper
    {
        // Structure and API declarations:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)]
string szPrinter, ref IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In(), MarshalAs(UnmanagedType.LPStruct)]
DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, ref Int32 dwWritten);

        private IntPtr hPrinter = new IntPtr(0);
        private DOCINFOA di = new DOCINFOA();

        private bool PrinterOpen = false;
        public bool PrinterIsOpen
        {
            get { return PrinterOpen; }
        }

        public bool OpenPrint(string szPrinterName)
        {
            if (PrinterOpen == false)
            {
                di.pDocName = ".NET RAW Document";
                di.pDataType = "RAW";

                if (OpenPrinter(szPrinterName.Normalize(), ref hPrinter, IntPtr.Zero))
                {
                    // Start a document.
                    if (StartDocPrinter(hPrinter, 1, di))
                    {
                        if (StartPagePrinter(hPrinter))
                        {
                            PrinterOpen = true;
                        }
                    }
                }
            }

            return PrinterOpen;
        }

        public void ClosePrint()
        {
            if (PrinterOpen)
            {
                EndPagePrinter(hPrinter);
                EndDocPrinter(hPrinter);
                ClosePrinter(hPrinter);
                PrinterOpen = false;
            }
        }

        public bool SendStringToPrinter(string szPrinterName, string szString)
        {
            bool functionReturnValue = false;
            if (PrinterOpen)
            {
                IntPtr pBytes = default(IntPtr);
                Int32 dwCount = default(Int32);
                Int32 dwWritten = 0;

                dwCount = szString.Length;
                //dwCount = asciiing.defuatl.getBytes(szString).length;

                pBytes = Marshal.StringToCoTaskMemAnsi(szString);
                //pBytes = Marshal.StringToCoTaskMemAuto(szString);
                //pBytes = Marshal.StringToCoTaskMemUni(szString);
                //pBytes = Marshal.StringToHGlobalUni(szString);
                
                

                functionReturnValue = WritePrinter(hPrinter, pBytes, dwCount, ref dwWritten);

                Marshal.FreeCoTaskMem(pBytes);
            }
            else
            {
                functionReturnValue = false;
            }
            return functionReturnValue;
        }
    }
}
