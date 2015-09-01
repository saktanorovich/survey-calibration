namespace Core.Calibration {
    public class SurveyPointPair {
        public int Seqno { get; private set; }
        public SurveyPoint Point1 { get; private set; }
        public SurveyPoint Point2 { get; private set; }

        public SurveyPointPair(int seqno, SurveyPoint point1, SurveyPoint point2) {
            Seqno = seqno;
            Point1 = point1;
            Point2 = point2;
        }
    }
}
