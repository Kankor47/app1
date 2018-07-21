using System;
using System.Collections.Generic;
using System.Text;

namespace FaceAnalyzer.Helpers
{
    public static class Constantes
    {
        public static readonly string FaceApiURL = "https://southcentralus.api.cognitive.microsoft.com/face/v1.0";
        public static readonly string FaceApiKey = "606b440e223740398c929dbb75773fbd";

        public static readonly string VisionApiURL = "https://southcentralus.api.cognitive.microsoft.com/vision/v1.0";
        public static readonly string VisionApiKey = "1ae1258435474a9e8fe5cbde48a4f77b";

        public static double LookingAwayAngleThreshold = 20;
        public static double YawningApertureThreshold = 0.2;
        public static double SleepingApertureThreshold = 0.45;
    }
}
