using RandomVariablesLibraryNew.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew
{
    public class ConvolutionRunner
    {
        public List<Tuple<Segment, Segment>> SegmentTuplesList { get; set; }

        public ConvolutionRunner(List<Tuple<Segment, Segment>> segments)
        {
            SegmentTuplesList = segments;
        }

        public double GetConvolutionValueAtPoint(double x)
        {
            double integralValue = default;

            foreach(var tuple in SegmentTuplesList)
            {
                var seg1 = tuple.Item1;
                var seg2 = tuple.Item2;

                var fun1 = GetFunc1(seg1, seg2, x);
                var fun2 = GetFunc2(seg1, seg2, x);
            }

            //var func1 = GetFunc1(

            return integralValue;
        }

        private Func<double, double> GetFunc1(Segment segment1, Segment segment2, double x)
        {
            return (t) => segment1[t] * segment2[x - t];
        }

        private Func<double, double> GetFunc2(Segment segment1, Segment segment2, double x)
        {
            return (t) => segment1[x - t] * segment2[t];
        }
    }
}
