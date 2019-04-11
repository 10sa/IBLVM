using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using CryptoStream;

namespace IBLVM_Libaray.Models
{
	/// <summary>
	/// IBLVM 통신에 사용되는 암호화 클래스 인스턴스 제공자입니다.
	/// </summary>
	public class CryptoProvider : IDisposable
	{
		public CryptoMemoryStream CryptoStream { get; set; }
		
		public ECDiffieHellmanCng ECDiffieHellman { get; set; }

		public void Dispose()
		{
			if (CryptoStream != null)
				CryptoStream.Dispose();

			if (ECDiffieHellman != null)
				ECDiffieHellman.Dispose();
		}
	}
}
