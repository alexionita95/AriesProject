using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Physics
{
    public class RigidBody
    {
        public float InverseMass { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Rotation { get; set; }
        public Matrix4x4 TransformMatrix { get; set; }
        public Quaternion Orientation { get; set; }

        public void CalculateTransformMatrix()
        {
            Matrix4x4 transform = Matrix4x4.Identity;
            transform.M11 = 1 - 2 * Orientation.Y * Orientation.Y - 2 * Orientation.Z * Orientation.Z;
            transform.M12 = 2 * Orientation.X * Orientation.Y - 2 * Orientation.W * Orientation.Z;
            transform.M13 = 2 * Orientation.X * Orientation.Z + 2 * Orientation.W * Orientation.Y;
            transform.M14 = Position.X;

            transform.M21 = 2 * Orientation.X * Orientation.Y + 2 * Orientation.W * Orientation.Z;
            transform.M22 = 1 - Orientation.X * Orientation.X - Orientation.Z * Orientation.Z;
            transform.M23 = 2 * Orientation.Y * Orientation.Z - 2 * Orientation.W * Orientation.X;
            transform.M24 = Position.Y;

            transform.M31 = 2 * Orientation.X * Orientation.Z - 2 * Orientation.W * Orientation.X;
            transform.M32 = 2*Orientation.Y*Orientation.Z + 2*Orientation.W*Orientation.X;
            transform.M33 = 1- 2*Orientation.X*Orientation.X - 2*Orientation.Y * Orientation.Y;
            transform.M34 = Position.Z;


            TransformMatrix = transform;

        }

        public void CalculateDerivedData()
        {
            CalculateTransformMatrix();
        }
    }
}
