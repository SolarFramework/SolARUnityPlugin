using SolAR.Datastructure;
using UnityEngine;

namespace SolAR
{
    public static class PoseExtensions
    {
        public static Pose ToUnity(this Transform3Df pose)
        {
            var inv = new Matrix4x4();
            inv.SetRow(0, new Vector4(+1, +0, +0, +0));
            inv.SetRow(1, new Vector4(+0, -1, +0, +0));
            inv.SetRow(2, new Vector4(+0, +0, +1, +0));
            inv.SetRow(3, new Vector4(+0, +0, +0, +1));

            var rot = pose.rotation();
            var trans = pose.translation();
            var m = new Matrix4x4();
            for (int r = 0; r < 3; ++r)
            {
                for (int c = 0; c < 3; ++c)
                {
                    m[r, c] = rot.coeff(r, c);
                }
                m[r, 3] = trans.coeff(r, 0);
            }
            m.SetRow(3, new Vector4(0, 0, 0, 1));

            m = m.inverse;

            m = inv * m;

            //m = m.inverse;

            var v = m.GetColumn(3);
            var q = Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));

            q = Quaternion.Inverse(q);
            v = q * -v;

            return new Pose(v, q);
        }
    }
}
