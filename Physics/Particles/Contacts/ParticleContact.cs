using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Particles.Contacts
{
    public class ParticleContact
    {
        public Particle Particle1 { get; set; }
        public Particle? Particle2 { get; set; } = null;
        public float Restitution { get; set; }
        public Vector3 ContactNormal { get; set; }
        public float Penetration { get; set; }

        public void Resolve(float duration)
        {
            ResolveVelocity(duration);
            ResolveInterpenetration(duration);
        }

        void ResolveInterpenetration(float duration)
        {
            if (Penetration <= 0)
            {
                return;
            }
            if (Particle1.Data.InfiniteMass && Particle2 != null && Particle2.Data.InfiniteMass)
            {
                return;
            }

            float totalInverseMass = Particle1.Data.InverseMass;
            if (Particle2 != null)
            {
                totalInverseMass += Particle2.Data.InverseMass;
            }

            Vector3 movePerMass = ContactNormal * (-Penetration / totalInverseMass);
            Particle1.Position = Particle1.Position + movePerMass * Particle1.Data.InverseMass;
            if (Particle2 != null)
            {
                Particle2.Position = Particle2.Position + movePerMass * Particle2.Data.InverseMass;
            }


        }

        void ResolveVelocity(float duration)
        {
            if (Particle1.Data.InfiniteMass && Particle2 != null && Particle2.Data.InfiniteMass)
            {
                return;
            }
            float separatingVelocity = CalculateSeparatingVelocity();
            if (separatingVelocity > 0)
            {
                return;
            }
            float newSeparatingVelocity = -separatingVelocity * Restitution;


            Vector3 accCausedVelocity = Particle1.Acceleration;
            if (Particle2 != null)
            {
                accCausedVelocity -= Particle2.Acceleration;
            }

            float accCausedSepVelocity = Vector3.Dot(accCausedVelocity, ContactNormal) * duration;
            if (accCausedSepVelocity < 0)
            {
                newSeparatingVelocity += Restitution * accCausedSepVelocity;
                if (newSeparatingVelocity < 0)
                {
                    newSeparatingVelocity = 0;
                }
            }

            float deltaVelocity = newSeparatingVelocity - separatingVelocity;

            float totalInverseMass = Particle1.Data.InverseMass;
            if (Particle2 != null)
            {
                totalInverseMass += Particle2.Data.InverseMass;
            }

            float impulse = deltaVelocity / totalInverseMass;

            Vector3 impulsePerMass = ContactNormal * impulse;

            Particle1.Velocity = Particle1.Velocity + impulsePerMass * Particle1.Data.InverseMass;

            if (Particle2 != null)
            {
                Particle2.Velocity = Particle2.Velocity + impulsePerMass * -Particle2.Data.InverseMass;
            }

        }

        public float CalculateSeparatingVelocity()
        {
            Vector3 relativeVelocity = Particle1.Velocity;
            if (Particle2 != null)
            {
                relativeVelocity -= Particle2.Velocity;
            }
            return Vector3.Dot(relativeVelocity, ContactNormal);
        }
    }
}
