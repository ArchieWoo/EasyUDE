using EasyUDE.Contracts;
using System.Text;

namespace EasyUDE.Helpers
{
    public class CharsetDetector: UniversalDetector, ICharsetDetector
    {
        private string charset;

        private float confidence;

        public void Feed(Stream stream)
        {
            byte[] buff = new byte[1024];
            int read;
            while ((read = stream.Read(buff, 0, buff.Length)) > 0 && !done)
            {
                Feed(buff, 0, read);
            }
        }
        public void Feed(string[,] strings)
        {
            string mergedString = MergeStrings(strings);
            using (Stream stream = ConvertToStream(mergedString))
            {
                Feed(stream);
            }
        }
        public void Feed(string[] strings)
        {
            string mergedString = MergeStrings(strings);
            using (Stream stream = ConvertToStream(mergedString))
            {
                Feed(stream);
            }
        }
        public void Feed(string inputString)
        {
            if(string.IsNullOrEmpty(inputString))
            {
                return;
            }
            using (Stream stream = ConvertToStream(inputString))
            {
                Feed(stream);
            }
        }
        public bool IsDone()
        {
            return done;
        }

        public override void Reset()
        {
            charset = null;
            confidence = 0.0f;
            base.Reset();
        }

        public string Charset
        {
            get { return charset; }
        }

        public float Confidence
        {
            get { return confidence; }
        }

        protected override void Report(string charset, float confidence)
        {
            this.charset = charset;
            this.confidence = confidence;
        }
        public CharsetDetector():base(FILTER_ALL)
        {
            
        }

        private static string MergeStrings(string[,] inputArray)
        {
            int rows = inputArray.GetLength(0);
            int cols = inputArray.GetLength(1);

            StringBuilder mergedBuilder = new StringBuilder();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    string value = inputArray[row, col];
                    if (!string.IsNullOrEmpty(value))
                    {
                        mergedBuilder.Append(value).Append(" ");
                    }
                }
            }
            return mergedBuilder.ToString().Trim();
        }
        private static string MergeStrings(string[] inputArray)
        {
            StringBuilder mergedBuilder = new StringBuilder();
            for (int i = 0; i < inputArray.Length; i++)
            {
                string value = inputArray[i];
                if (!string.IsNullOrEmpty(value))
                {
                    mergedBuilder.Append(value).Append(" ");
                }
            }
            return mergedBuilder.ToString().Trim();
        }
        private Stream ConvertToStream(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return new MemoryStream();
            }
            byte[] byteArray = Encoding.UTF8.GetBytes(inputString);
            return new MemoryStream(byteArray);
        }
    }
}
