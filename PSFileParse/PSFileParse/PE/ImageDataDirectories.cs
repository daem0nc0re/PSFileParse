using System;

namespace PSFileParse.PE
{
    public sealed class ImageDataDirectories
    {
        public ImageDataDirectory Export { get; }
        public ImageDataDirectory Import { get; }
        public ImageDataDirectory Resource { get; }
        public ImageDataDirectory Exception { get; }
        public ImageDataDirectory Security { get; }
        public ImageDataDirectory BaseReloc { get; }
        public ImageDataDirectory Debug { get; }
        public ImageDataDirectory Architecture { get; }
        public ImageDataDirectory GlobalPtr { get; }
        public ImageDataDirectory TLS { get; }
        public ImageDataDirectory LoadConfig { get; }
        public ImageDataDirectory BoundImport { get; }
        public ImageDataDirectory IAT { get; }
        public ImageDataDirectory DelayImport { get; }
        public ImageDataDirectory CLR { get; }
        public ImageDataDirectory Reserved { get; }


        internal ImageDataDirectories(byte[] filebytes, UInt32 offset)
        {
            Export = new ImageDataDirectory(filebytes, offset);
            Import = new ImageDataDirectory(filebytes, offset + 8);
            Resource = new ImageDataDirectory(filebytes, offset + 16);
            Exception = new ImageDataDirectory(filebytes, offset + 24);
            Security = new ImageDataDirectory(filebytes, offset + 32);
            BaseReloc = new ImageDataDirectory(filebytes, offset + 40);
            Debug = new ImageDataDirectory(filebytes, offset + 48);
            Architecture = new ImageDataDirectory(filebytes, offset + 56);
            GlobalPtr = new ImageDataDirectory(filebytes, offset + 64);
            TLS = new ImageDataDirectory(filebytes, offset + 72);
            LoadConfig = new ImageDataDirectory(filebytes, offset + 80);
            BoundImport = new ImageDataDirectory(filebytes, offset + 88);
            IAT = new ImageDataDirectory(filebytes, offset + 96);
            DelayImport = new ImageDataDirectory(filebytes, offset + 104);
            CLR = new ImageDataDirectory(filebytes, offset + 112);
            Reserved = new ImageDataDirectory(filebytes, offset + 120);
        }


        public override String ToString()
        {
            return String.Format("@{{Export={0}; Import={1}; Resource={2}; Exception={3}; Security={4}; BaseReloc={5}; Debug={6}; Architecture={7}; GlobalPtr={8}; TLS={9}; LoadConfig={10}; BoundImport={11}; IAT={12}; DelayImport={13}; CLR={14}; Reserved={15}}}",
                Export,
                Import,
                Resource,
                Exception,
                Security,
                BaseReloc,
                Debug,
                Architecture,
                GlobalPtr,
                TLS,
                LoadConfig,
                BoundImport,
                IAT,
                DelayImport,
                CLR,
                Reserved);
        }
    }
}
