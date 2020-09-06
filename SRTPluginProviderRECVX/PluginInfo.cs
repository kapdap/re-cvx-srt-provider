using SRTPluginBase;
using System;

namespace SRTPluginProviderRECVX
{
    public class PluginInfo : IPluginInfo
    {
        public string Name => "Game Memory Provider (Resident Evil: Code: Veronica)";

        public string Description => "A game memory provider plugin for Resident Evil: Code: Veronica.";

        public string Author => "Kapdap";

        public Uri MoreInfoURL => new Uri("https://github.com/kapdap/re-cvx-srt-provider");

        public int VersionMajor => assemblyFileVersion.ProductMajorPart;

        public int VersionMinor => assemblyFileVersion.ProductMinorPart;

        public int VersionBuild => assemblyFileVersion.ProductBuildPart;

        public int VersionRevision => assemblyFileVersion.ProductPrivatePart;

        private System.Diagnostics.FileVersionInfo assemblyFileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
    }
}