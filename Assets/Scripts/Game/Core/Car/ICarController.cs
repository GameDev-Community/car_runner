using System;

namespace Game.Core.Car
{
    public interface ICarController
    {
        public float MaxSpeed { get; set; }
        public float Speed { get; set; }
        public float Acceleration { get; set; }
        public float VerticalMoving { get; }
        public float HorizontalMoving { get; }

        public bool Grounded { get; }


        public void SetVerticalMoving(float v);
        public void SetHorizontalMoving(float v);
    }
}
