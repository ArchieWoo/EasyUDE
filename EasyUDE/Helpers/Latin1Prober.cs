namespace EasyUDE.Helpers
{
    // TODO: Using trigrams the detector should be able to discriminate between 
    // latin-1 and iso8859-2
    public class Latin1Prober : CharsetProber
    {
        private const int FREQ_CAT_NUM = 4;

        private const int UDF = 0;       // undefined
        private const int OTH = 1;       // other
        private const int ASC = 2;       // ascii capital letter
        private const int ASS = 3;       // ascii small letter
        private const int ACV = 4;       // accent capital vowel
        private const int ACO = 5;       // accent capital other
        private const int ASV = 6;       // accent small vowel
        private const int ASO = 7;       // accent small other
        
        private const int CLASS_NUM = 8; // total classes
        
        private readonly static byte[] Latin1_CharToClass = {
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // 00 - 07
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // 08 - 0F
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // 10 - 17
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // 18 - 1F
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // 20 - 27
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // 28 - 2F
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // 30 - 37
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // 38 - 3F
          OTH, ASC, ASC, ASC, ASC, ASC, ASC, ASC,   // 40 - 47
          ASC, ASC, ASC, ASC, ASC, ASC, ASC, ASC,   // 48 - 4F
          ASC, ASC, ASC, ASC, ASC, ASC, ASC, ASC,   // 50 - 57
          ASC, ASC, ASC, OTH, OTH, OTH, OTH, OTH,   // 58 - 5F
          OTH, ASS, ASS, ASS, ASS, ASS, ASS, ASS,   // 60 - 67
          ASS, ASS, ASS, ASS, ASS, ASS, ASS, ASS,   // 68 - 6F
          ASS, ASS, ASS, ASS, ASS, ASS, ASS, ASS,   // 70 - 77
          ASS, ASS, ASS, OTH, OTH, OTH, OTH, OTH,   // 78 - 7F
          OTH, UDF, OTH, ASO, OTH, OTH, OTH, OTH,   // 80 - 87
          OTH, OTH, ACO, OTH, ACO, UDF, ACO, UDF,   // 88 - 8F
          UDF, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // 90 - 97
          OTH, OTH, ASO, OTH, ASO, UDF, ASO, ACO,   // 98 - 9F
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // A0 - A7
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // A8 - AF
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // B0 - B7
          OTH, OTH, OTH, OTH, OTH, OTH, OTH, OTH,   // B8 - BF
          ACV, ACV, ACV, ACV, ACV, ACV, ACO, ACO,   // C0 - C7
          ACV, ACV, ACV, ACV, ACV, ACV, ACV, ACV,   // C8 - CF
          ACO, ACO, ACV, ACV, ACV, ACV, ACV, OTH,   // D0 - D7
          ACV, ACV, ACV, ACV, ACV, ACO, ACO, ACO,   // D8 - DF
          ASV, ASV, ASV, ASV, ASV, ASV, ASO, ASO,   // E0 - E7
          ASV, ASV, ASV, ASV, ASV, ASV, ASV, ASV,   // E8 - EF
          ASO, ASO, ASV, ASV, ASV, ASV, ASV, OTH,   // F0 - F7
          ASV, ASV, ASV, ASV, ASV, ASO, ASO, ASO,   // F8 - FF
        };

        /* 0 : illegal 
           1 : very unlikely 
           2 : normal 
           3 : very likely
        */
        private readonly static byte[] Latin1ClassModel = {
            /*      UDF OTH ASC ASS ACV ACO ASV ASO  */
            /*UDF*/  0,  0,  0,  0,  0,  0,  0,  0,
            /*OTH*/  0,  3,  3,  3,  3,  3,  3,  3,
            /*ASC*/  0,  3,  3,  3,  3,  3,  3,  3, 
            /*ASS*/  0,  3,  3,  3,  1,  1,  3,  3,
            /*ACV*/  0,  3,  3,  3,  1,  2,  1,  2,
            /*ACO*/  0,  3,  3,  3,  3,  3,  3,  3, 
            /*ASV*/  0,  3,  1,  3,  1,  1,  1,  3, 
            /*ASO*/  0,  3,  1,  3,  1,  1,  3,  3,
        };

        private byte lastCharClass;
        private int[] freqCounter = new int[FREQ_CAT_NUM];
        
        public Latin1Prober()
        {
            Reset();
        }

        public override string GetCharsetName() 
        {
            return "windows-1252";
        }
        
        public override void Reset()
        {
            state = ProbingState.Detecting;
            lastCharClass = OTH;
            for (int i = 0; i < FREQ_CAT_NUM; i++)
                freqCounter[i] = 0;
        }
        
        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            byte[] newbuf = FilterWithEnglishLetters(buf, offset, len);
            byte charClass, freq;
            
            for (int i = 0; i < newbuf.Length; i++) {
                charClass = Latin1_CharToClass[newbuf[i]];
                freq = Latin1ClassModel[lastCharClass * CLASS_NUM + charClass];
                if (freq == 0) {
                  state = ProbingState.NotMe;
                  break;
                }
                freqCounter[freq]++;
                lastCharClass = charClass;
            }
            return state;
        }

        public override float GetConfidence()
        {
            if (state == ProbingState.NotMe)
                return 0.01f;
            
            float confidence = 0.0f;
            int total = 0;
            for (int i = 0; i < FREQ_CAT_NUM; i++) {
                total += freqCounter[i];
            }
            
            if (total <= 0) {
                confidence = 0.0f;
            } else {
                confidence = freqCounter[3] * 1.0f / total;
                confidence -= freqCounter[1] * 20.0f / total;
            }
            
            // lower the confidence of latin1 so that other more accurate detector 
            // can take priority.
            return confidence < 0.0f ? 0.0f : confidence * 0.5f;
        }

        public override string DumpStatus()
        {
            return $"Latin1Prober: {GetConfidence()} [{GetCharsetName()}]";
        }
    }
}

