using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;

namespace LatteMarche.Xamarin.Zebra
{
    public class Printer : IPrinter
    {
        #region Fields

        private IConnectionManager connectionManager = DependencyService.Get<IConnectionManager>();
        private Connection connection;

        #endregion

        #region Properties

        public string MacAddress { get; set; }

        #endregion

        #region Methods

        public Task PrintLabel(Registro registro)
        {
            try
            {
                Debug.WriteLine("Connecting");
                this.Connect();
                Debug.WriteLine("Connected");

                var labelMaker = this.MakeLabelMaker(registro);
                Debug.WriteLine("Label maked done");

                var labelDefinition = labelMaker.MakeLabel(registro);
                Debug.WriteLine($"Label definition: {labelDefinition}");

                this.PrintLabel(labelDefinition);
            }
            catch (Exception exc)
            {
                Debug.WriteLine($"Print error {exc.Message}");
                throw exc;
            }
            finally
            {
                Debug.WriteLine($"Disconnecting");
                this.Disconnect();
                Debug.WriteLine($"Disconnected");
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Factory label maker
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private ILabelMaker MakeLabelMaker(Registro registro)
        {
            //Debug.WriteLine($"Disconnecting");
            //var printerLanguage = ZebraPrinterFactory.GetInstance(this.connection).PrinterControlLanguage;

            //if (printerLanguage == PrinterLanguage.ZPL)
            //    return new ZplLabelMaker();
            //else
            if(registro is RegistroConsegna)
                return new CPCL.RegistroConsegnaMaker();

            if (registro is RegistroRaccolta)
                return new CPCL.RegistroRaccoltaMaker();

            return null;
        }

        /// <summary>
        /// Invio comando alla stampante
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="labelDefinition"></param>
        private void PrintLabel(string labelDefinition)
        {
            Debug.WriteLine($"Get printer instance (connection: {this.connection.Connected})");

            ZebraPrinter printer = ZebraPrinterFactory.GetInstance(this.connection);
;
            Debug.WriteLine($"Printer instance got");

            Debug.WriteLine($"Creating file");
            var filePath = CreateFile(labelDefinition);
            Debug.WriteLine($"File created {filePath}");

            Debug.WriteLine($"Contents sending");
            printer.SendFileContents(filePath);
            Debug.WriteLine($"Contents sent");
        }

        private void Connect()
        {
            this.connection = this.connectionManager.GetBluetoothConnection(this.MacAddress);
            this.connection.Open();
        }

        private void Disconnect()
        {
            this.connection.Close();
        }

        private string CreateFile(string labelDef)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), "TEST_ZEBRA.LBL");
            using (FileStream tempFileStream = new FileStream(tempFilePath, FileMode.Create))
            {
                byte[] testLabelBytes = Encoding.UTF8.GetBytes(labelDef);
                tempFileStream.Write(testLabelBytes, 0, testLabelBytes.Length);
                tempFileStream.Flush();
            }

            return new FileInfo(tempFilePath).FullName;
        }


        #endregion

    }
}
