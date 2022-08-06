using System;
using System.Collections.Generic;

namespace EventCartoViewer
{
    public class Coord
    {
        public double Y { get; set; }
        public double X { get; set; }
        public string Zone { get; set; }
        public int Fuseau { get; set; }

        public static Coord FromEventCoord(EventCoord ec)
        {
            return new Coord
            {
                X = ec.X,
                Y = ec.Y
            };
        }
    }

    public class EventCoord
    {
        public double Y { get; set; }
        public double X { get; set; }

        public static EventCoord FromCoord(Coord c)
        {
            return new EventCoord
            {
                X = c.X,
                Y = c.Y
            };
        }

        private static EventCoord ecCenter = new EventCoord { X = 0, Y = 0 };

        public static List<EventCoord> TriRandom(List<EventCoord> ecs)
        {
            Random rnd = new Random();
            List<EventCoord> ecsDest = new List<EventCoord>();

            do
            {
                int idx = rnd.Next(0, ecs.Count);
                ecsDest.Add(ecs[idx]);
                ecs.RemoveAt(idx);

            } while (ecs.Count > 0);
            
            return ecsDest;
        }

        public static List<EventCoord> TriCoord(List<EventCoord> ecs)
        {
            ecCenter.X = ecCenter.Y = 0;

            //Get center
            for (int i = 0; i < ecs.Count; i++)
            {
                ecCenter.X += ecs[i].X;
                ecCenter.Y += ecs[i].Y;
            }
            ecCenter.X = ecCenter.X / ecs.Count;
            ecCenter.Y = ecCenter.Y / ecs.Count;


            ecs.Sort(ComparePoints);
            return ecs;
        }

        private static int ComparePoints(EventCoord ec1, EventCoord ec2)
        {
            double a1 = GetAngle(ecCenter, ec1);
            double a2 = GetAngle(ecCenter, ec2);

            if (a1 < a2) return 1;

            double d1 = GetDistance(ecCenter, ec1);
            double d2 = GetDistance(ecCenter, ec2);

            if (a1 == a2 && d1 < d2) return 1;
            return -1;
        }

        private static double GetAngle(EventCoord ecCenter, EventCoord ec)
        {
            double x = ec.X - ecCenter.X;
            double y = ec.Y - ecCenter.Y;

            double a = Math.Atan2(x, y);
            if (a <= 0) a = 2 * Math.PI + a;
            return a;
        }

        private static double GetDistance(EventCoord ec1, EventCoord ec2)
        {
            double x = ec1.X - ec2.X;
            double y = ec1.Y - ec2.Y;
            return Math.Sqrt(x * x + y * y);
        }

    }
}
