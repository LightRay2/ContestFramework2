using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class Animator<T>
    {
        public T start, finish;
        public double startTime, duration;
        public double finishTime { get { return startTime + duration; } set { duration = value - startTime; } }
        public Func<T, T, double, T> swingFunction;

        //todo откомментировать
        public Animator(Func<T, T, double, T> swingFunction, T start, T finish, double duration, double startTime = 0)
        {
            this.swingFunction = swingFunction;
            this.start = start;
            this.duration = duration;
            this.finish = finish;
            this.startTime = startTime;
        }

        public T Get(double currentTime)
        {
            double stage = ((double)(currentTime - startTime) / (duration)).ToRange(0,1);
            return swingFunction(start, finish, stage);
          //  return (dynamic)left.Evaluate(context) + (dynamic)right.Evaluate(context);
        }

        List<Tuple<double, T>> interpolationValues = new List<Tuple<double, T>>();
        public void AddValueForInterpolation( double stage, T val)
        {
            interpolationValues.Add(Tuple.Create(stage, val));

        }
        
    }

    /// <summary>
    /// easing http://easings.net/ru http://stackoverflow.com/questions/5207301/jquery-easing-functions-without-using-a-plugin
    /// </summary>
    public static class Animator
    {
        //todo добавить swing функций
        /// <summary>
        /// метод для вычисления плавного ускорения и торможения, стартовой точке соответствует stage = 0, финишной - stage = 1
        /// </summary>
        public static double SinSwingRefined(double start, double finish, double stage)
        {
            var append = finish - start;
            double term, k = 1.8, acc = 0.15;
            if (stage < acc) term = (-Math.Cos(stage / k * Math.PI) / 2) + 0.5;
            else if (stage < 1 - acc) term = ((-Math.Cos(((stage - 0.5) / ((0.5 - acc) / (0.5 - acc / k)) + 0.5) * Math.PI) / 2) + 0.5);
            else term = ((-Math.Cos(((stage - 1) / k + 1) * Math.PI) / 2) + 0.5);
            return start + append * term;
        }
        //просто копипаст
        public static Vector2d SinSwingRefined(Vector2d start, Vector2d finish, double stage)
        {
            var append = finish - start;
            double term, k = 1.8, acc = 0.15;
            if (stage < acc) term = (-Math.Cos(stage / k * Math.PI) / 2) + 0.5;
            else if (stage < 1 - acc) term = ((-Math.Cos(((stage - 0.5) / ((0.5 - acc) / (0.5 - acc / k)) + 0.5) * Math.PI) / 2) + 0.5);
            else term = ((-Math.Cos(((stage - 1) / k + 1) * Math.PI) / 2) + 0.5);
            return start + append * term;
        }

        public static double EaseOutQuad(double start, double finish, double stage)
        {
            var part = 1.0 - (1-stage)*(1-stage);
            return start + part * (finish-start);
        }

        public static Vector2d Linear(Vector2d start, Vector2d finish, double stage)
        {
            var dif = finish - start;
            return start + dif * stage;
        }
        public static double Linear(double start, double finish, double stage)
        {
            var dif = finish - start;
            return start + dif * stage;
        }
    }

    public class InterpolationFunction
    {
        private Vector2d _initialPoint;
        private double _stageStep;

        public InterpolationFunction(Vector2d initialPoint, double stageStepBetween0and1)
        {
            _initialPoint = initialPoint;
            _stageStep = stageStepBetween0and1;
            _points.Add(initialPoint);
        }

        List<Vector2d> _points = new List<Vector2d>();
        public void Add(Vector2d point)
        {
            _points.Add(point);
        }

        public Vector2d GetInterpolated(Vector2d start, Vector2d finish, double stage)
        {
           
            int one = (int)(stage / _stageStep);
            if (one < 0)
                one = 0;
            if (one >= _points.Count - 1)
                return _points.Last();

            int two = one + 1;

            return Animator.Linear(_points[one], _points[two], stage / _stageStep - one);
        }
    }
}
