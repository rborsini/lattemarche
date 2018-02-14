using System.IO;

namespace LatteMarche.SITRA.Jobs
{
    public class ClearTempFolderJob : BaseJob
    {
        #region Properties

        public string TempFolder { get { return base.Params["tempFolder"]; } }

        #endregion

        #region Constructors

        public ClearTempFolderJob()
            : base() { }

        #endregion

        #region Methods

        public override void Execute()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(this.TempFolder);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
                log.Info("Removed file " + file.Name);
            }

            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                directory.Delete(true);
                log.Info("Removed directory " + directory.Name);
            }
        }

        #endregion
    }
}