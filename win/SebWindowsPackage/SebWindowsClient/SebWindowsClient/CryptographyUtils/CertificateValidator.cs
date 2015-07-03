using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace SebWindowsClient.CryptographyUtils
{
	public static class CertificateValidator
	{
		public static string CommonName { get; set; }

		public static void SetUntrustedRootCertificateValidation()
		{
			System.Net.ServicePointManager.ServerCertificateValidationCallback = UntrustedRootTestCertificate;
		}

		public static void SetTrustedCertificateValidation()
		{
			System.Net.ServicePointManager.ServerCertificateValidationCallback = TrustedRootTestCertificate;
		}

		private static bool TrustedRootTestCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return sslPolicyErrors == SslPolicyErrors.None && (string.IsNullOrEmpty(CommonName) || certificate.Subject.Contains(CommonName));
		}

		private static bool UntrustedRootTestCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			// If the certificate is a valid, signed certificate, return true.
			if(sslPolicyErrors == SslPolicyErrors.None)
			{
				return true;
			}

			// If there are errors in the certificate chain, look at each error to determine the cause.
			if((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
			{
				if(chain != null)
				{
					foreach(var status in chain.ChainStatus)
					{
						// Self-signed certificates with an untrusted root are valid. 
						if(!((certificate.Subject == certificate.Issuer) && (status.Status == X509ChainStatusFlags.UntrustedRoot)))
						{
							if(status.Status != X509ChainStatusFlags.NoError)
							{
								// If there are any other errors in the certificate chain, the certificate is invalid,
								// so the method returns false.
								return false;
							}
						}
					}
				}

				// When processing reaches this line, the only errors in the certificate chain are 
				// untrusted root errors for self-signed certificates. These certificates are valid
				// for default Exchange server installations, so return true.
				return true;
			}
			else
			{
				// In all other cases, return false.
				return false;
			}
		}

	}
}