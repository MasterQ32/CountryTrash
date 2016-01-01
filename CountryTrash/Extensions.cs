using Lidgren.Network;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryTrash
{
	public static class sExtensions
	{
		public static string Nullify(this string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return null;
			return text;
		}

		public static void Write(this NetOutgoingMessage msg, Vector2 vec)
		{
			msg.Write(vec.X);
			msg.Write(vec.Y);
		}

		public static void Write(this NetOutgoingMessage msg, Vector3 vec)
		{
			msg.Write(vec.X);
			msg.Write(vec.Y);
			msg.Write(vec.Z);
		}

		public static void Write(this NetOutgoingMessage msg, Vector4 vec)
		{
			msg.Write(vec.X);
			msg.Write(vec.Y);
			msg.Write(vec.Z);
			msg.Write(vec.W);
		}

		public static Vector2 ReadVector2(this NetIncomingMessage msg)
		{
			var vec = new Vector2();
			msg.ReadSingle(out vec.X);
			msg.ReadSingle(out vec.Y);
			return vec;
		}

		public static Vector3 ReadVector3(this NetIncomingMessage msg)
		{
			var vec = new Vector3();
			msg.ReadSingle(out vec.X);
			msg.ReadSingle(out vec.Y);
			msg.ReadSingle(out vec.Z);
			return vec;
		}

		public static Vector4 ReadVector4(this NetIncomingMessage msg)
		{
			var vec = new Vector4();
			msg.ReadSingle(out vec.X);
			msg.ReadSingle(out vec.Y);
			msg.ReadSingle(out vec.Z);
			msg.ReadSingle(out vec.W);
			return vec;
		}
	}
}
