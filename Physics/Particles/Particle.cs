using System;
using System.Collections.Generic;
using System.Numerics;
using Physics.Particles.Generators;

namespace Physics.Particles
{
    public class Data
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Acceleration { get; set; }
        public float Mass { get { return _mass; } set { _mass = value; _inverseMass = 1 / _mass; } }
        public float InverseMass { get { return _inverseMass; } set { _inverseMass = value; } }
        public float Damping { get; set; }
        public Vector3 Accumulator { get; set; }
        private float _mass;
        private float _inverseMass;
        public bool InfiniteMass { get; set; } = false;
    }
    public class Particle
    {
        public Data Data { get; set; }
        public Vector3 Position { get { return Data.Position; } set { Data.Position = value; } }
        public Vector3 Velocity { get; set; }
        public Vector3 Acceleration { get; set; }
        public float Mass { get { return Data.Mass; } set { Data.Mass = value; } }
        public float Damping { get; set; }

        List<ForceGenerator> generators;

        public Particle()
        {
            Data = new Data();
            generators = new List<ForceGenerator>();
        }

        public void AddGenerator(ForceGenerator gen)
        {
            if (!generators.Contains(gen))
            {
                generators.Add(gen);
            }

        }

        private void CalculateForces(float duration)
        {
            foreach (var gen in generators)
            {
                var force = gen.Update(Data, duration);
                AddForce(force);

            }
        }
        public void AddForce(Vector3 force)
        {
            Data.Accumulator += force;
        }

        public void Integrate(float duration)
        {
            ClearAccumulator();
            CalculateForces(duration);
            Position += duration * Velocity;

            Vector3 resultAcc = Acceleration;

            resultAcc += Data.InverseMass * Data.Accumulator;

            Velocity += duration * resultAcc;
            //Velocity *= MathF.Pow(Damping, duration);

        }

        void ClearAccumulator()
        {
            Data.Accumulator = Vector3.Zero;
        }
    }
}
