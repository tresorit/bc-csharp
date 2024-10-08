using System;
using System.IO;

using NUnit.Framework;

using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Utilities.Test;

namespace Org.BouncyCastle.Asn1.Tests
{
	[TestFixture]
	public class OcspTest
		: ITest
	{
		private static byte[] unsignedReq = Base64.Decode(
			"MEIwQDA+MDwwOjAJBgUrDgMCGgUABBRDb9GODnq7lRhSkEqw4XX24huERwQUkY4j"
			+ "a6eKuDlkVP9hRgkEvIWqHPECAQE=");

		private static byte[] signedReq = Base64.Decode(
			"MIIC9jBAMD4wPDA6MAkGBSsOAwIaBQAEFENv0Y4OeruVGFKQSrDhdfbiG4RHBBTc"
			+ "Mr1fP+mZAxbF2ZdehWxn6mtAngIBAaCCArAwggKsMA0GCSqGSIb3DQEBBQUAA4GB"
			+ "AAzHBm4nL5AcRQB3Jkz7ScNeZF+GbRZ0p4kBDTnqi3IeESuso12yJhpqqyijdnj5"
			+ "gd4/GsSAgdluLHyYZ6wgozV7G9MDXCnFnG4PBUW05HaVX81JYAp+amVyU0NOgNrG"
			+ "90npVBsHb0o+UlkxNgMiEbSkp/TeGb6YURsYKhmwp7BgoIICFTCCAhEwggINMIIB"
			+ "dqADAgECAgEBMA0GCSqGSIb3DQEBBAUAMCUxFjAUBgNVBAoTDUJvdW5jeSBDYXN0"
			+ "bGUxCzAJBgNVBAYTAkFVMB4XDTA0MTAyNDEzNDc0M1oXDTA1MDIwMTEzNDc0M1ow"
			+ "JTEWMBQGA1UEChMNQm91bmN5IENhc3RsZTELMAkGA1UEBhMCQVUwgZ8wDQYJKoZI"
			+ "hvcNAQEBBQADgY0AMIGJAoGBAJBmLeIzthMHUeTkOeJ76iBxcMHY31o/i3a9VT12"
			+ "y2FcS/ejJmeUCMTdtwl5alOwXY66vF4DyT1VU/nJG3mHpSoqq7qrMXOIFGcXg1Wf"
			+ "oJRrQgTOLdQ6bod7i9ME/EjEJy70orh0nVS7NGcu0R5TjcbLde2J5zxjb/W9wqfy"
			+ "RovJAgMBAAGjTTBLMB0GA1UdDgQWBBTcMr1fP+mZAxbF2ZdehWxn6mtAnjAfBgNV"
			+ "HSMEGDAWgBTcMr1fP+mZAxbF2ZdehWxn6mtAnjAJBgNVHRMEAjAAMA0GCSqGSIb3"
			+ "DQEBBAUAA4GBAF/4EH1KkNrNxocJPIp7lThmG1KIVYESIadowMowrbok46ESofRF"
			+ "OIPku07W+e1Y1Y1KXLIiPMG3IGwrBrn04iLsbbBUiN37BcC/VyT4xKJ2MYscGjKL"
			+ "ua/9bU0lOyeTRAwqb8towWRd5lLYAI3RQ7dhStUTFp3Vqd803PJ/cpR6");

		private static byte[] _response = Base64.Decode(
			"MIIFnAoBAKCCBZUwggWRBgkrBgEFBQcwAQEEggWCMIIFfjCCARehgZ8wgZwx"
			+ "CzAJBgNVBAYTAklOMRcwFQYDVQQIEw5BbmRocmEgcHJhZGVzaDESMBAGA1UE"
			+ "BxMJSHlkZXJhYmFkMQwwCgYDVQQKEwNUQ1MxDDAKBgNVBAsTA0FUQzEeMBwG"
			+ "A1UEAxMVVENTLUNBIE9DU1AgUmVzcG9uZGVyMSQwIgYJKoZIhvcNAQkBFhVv"
			+ "Y3NwQHRjcy1jYS50Y3MuY28uaW4YDzIwMDMwNDAyMTIzNDU4WjBiMGAwOjAJ"
			+ "BgUrDgMCGgUABBRs07IuoCWNmcEl1oHwIak1BPnX8QQUtGyl/iL9WJ1VxjxF"
			+ "j0hAwJ/s1AcCAQKhERgPMjAwMjA4MjkwNzA5MjZaGA8yMDAzMDQwMjEyMzQ1"
			+ "OFowDQYJKoZIhvcNAQEFBQADgYEAfbN0TCRFKdhsmvOdUoiJ+qvygGBzDxD/"
			+ "VWhXYA+16AphHLIWNABR3CgHB3zWtdy2j7DJmQ/R7qKj7dUhWLSqclAiPgFt"
			+ "QQ1YvSJAYfEIdyHkxv4NP0LSogxrumANcDyC9yt/W9yHjD2ICPBIqCsZLuLk"
			+ "OHYi5DlwWe9Zm9VFwCGgggPMMIIDyDCCA8QwggKsoAMCAQICAQYwDQYJKoZI"
			+ "hvcNAQEFBQAwgZQxFDASBgNVBAMTC1RDUy1DQSBPQ1NQMSYwJAYJKoZIhvcN"
			+ "AQkBFhd0Y3MtY2FAdGNzLWNhLnRjcy5jby5pbjEMMAoGA1UEChMDVENTMQww"
			+ "CgYDVQQLEwNBVEMxEjAQBgNVBAcTCUh5ZGVyYWJhZDEXMBUGA1UECBMOQW5k"
			+ "aHJhIHByYWRlc2gxCzAJBgNVBAYTAklOMB4XDTAyMDgyOTA3MTE0M1oXDTAz"
			+ "MDgyOTA3MTE0M1owgZwxCzAJBgNVBAYTAklOMRcwFQYDVQQIEw5BbmRocmEg"
			+ "cHJhZGVzaDESMBAGA1UEBxMJSHlkZXJhYmFkMQwwCgYDVQQKEwNUQ1MxDDAK"
			+ "BgNVBAsTA0FUQzEeMBwGA1UEAxMVVENTLUNBIE9DU1AgUmVzcG9uZGVyMSQw"
			+ "IgYJKoZIhvcNAQkBFhVvY3NwQHRjcy1jYS50Y3MuY28uaW4wgZ8wDQYJKoZI"
			+ "hvcNAQEBBQADgY0AMIGJAoGBAM+XWW4caMRv46D7L6Bv8iwtKgmQu0SAybmF"
			+ "RJiz12qXzdvTLt8C75OdgmUomxp0+gW/4XlTPUqOMQWv463aZRv9Ust4f8MH"
			+ "EJh4ekP/NS9+d8vEO3P40ntQkmSMcFmtA9E1koUtQ3MSJlcs441JjbgUaVnm"
			+ "jDmmniQnZY4bU3tVAgMBAAGjgZowgZcwDAYDVR0TAQH/BAIwADALBgNVHQ8E"
			+ "BAMCB4AwEwYDVR0lBAwwCgYIKwYBBQUHAwkwNgYIKwYBBQUHAQEEKjAoMCYG"
			+ "CCsGAQUFBzABhhpodHRwOi8vMTcyLjE5LjQwLjExMDo3NzAwLzAtBgNVHR8E"
			+ "JjAkMCKgIKAehhxodHRwOi8vMTcyLjE5LjQwLjExMC9jcmwuY3JsMA0GCSqG"
			+ "SIb3DQEBBQUAA4IBAQB6FovM3B4VDDZ15o12gnADZsIk9fTAczLlcrmXLNN4"
			+ "PgmqgnwF0Ymj3bD5SavDOXxbA65AZJ7rBNAguLUo+xVkgxmoBH7R2sBxjTCc"
			+ "r07NEadxM3HQkt0aX5XYEl8eRoifwqYAI9h0ziZfTNes8elNfb3DoPPjqq6V"
			+ "mMg0f0iMS4W8LjNPorjRB+kIosa1deAGPhq0eJ8yr0/s2QR2/WFD5P4aXc8I"
			+ "KWleklnIImS3zqiPrq6tl2Bm8DZj7vXlTOwmraSQxUwzCKwYob1yGvNOUQTq"
			+ "pG6jxn7jgDawHU1+WjWQe4Q34/pWeGLysxTraMa+Ug9kPe+jy/qRX2xwvKBZ");

		private ITestResult unSignedRequest()
		{
			try
			{
				OcspRequest req = OcspRequest.GetInstance(
					Asn1Object.FromByteArray(unsignedReq));

				if (!Arrays.AreEqual(req.GetEncoded(), unsignedReq))
				{
					return new SimpleTestResult(false, Name + ": Ocsp unsigned request failed to re-encode");
				}

				return new SimpleTestResult(true, Name + ": Okay");
			}
			catch (Exception e)
			{
				return new SimpleTestResult(false, Name + ": failed unsigned exception - " + e.ToString(), e);
			}
		}

		private ITestResult SignedRequest()
		{
			try
			{
				OcspRequest req = OcspRequest.GetInstance(
					Asn1Object.FromByteArray(signedReq));

				if (!Arrays.AreEqual(req.GetEncoded(), signedReq))
				{
					return new SimpleTestResult(false, Name + ": Ocsp signed request failed to re-encode");
				}

				return new SimpleTestResult(true, Name + ": Okay");
			}
			catch (Exception e)
			{
				return new SimpleTestResult(false, Name + ": failed signed exception - " + e.ToString(), e);
			}
		}

		private ITestResult Response()
		{
			try
			{
				OcspResponse resp = OcspResponse.GetInstance(
					Asn1Object.FromByteArray(_response));
				ResponseBytes rBytes = ResponseBytes.GetInstance(resp.ResponseBytes);

				BasicOcspResponse bResp = BasicOcspResponse.GetInstance(
					Asn1Object.FromByteArray(rBytes.Response.GetOctets()));

				resp = new OcspResponse(
					resp.ResponseStatus,
					new ResponseBytes(
						rBytes.ResponseType,
						new DerOctetString(bResp.GetEncoded())));

				if (!Arrays.AreEqual(resp.GetEncoded(), _response))
				{
					return new SimpleTestResult(false, Name + ": Ocsp response failed to re-encode");
				}

				return new SimpleTestResult(true, Name + ": Okay");
			}
			catch (Exception e)
			{
				return new SimpleTestResult(false, Name + ": failed response exception - " + e.ToString(), e);
			}
		}

		public ITestResult Perform()
		{
			ITestResult res = unSignedRequest();

			if (!res.IsSuccessful())
			{
				return res;
			}

			res = SignedRequest();
			if (!res.IsSuccessful())
			{
				return res;
			}

			return Response();
		}

		public string Name
		{
			get { return "Ocsp"; }
		}

		[Test]
		public void TestFunction()
		{
			string resultText = Perform().ToString();

			Assert.AreEqual(Name + ": Okay", resultText);
		}
	}
}
