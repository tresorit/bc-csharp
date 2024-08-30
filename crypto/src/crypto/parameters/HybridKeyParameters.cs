﻿using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC.Custom.Sec;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Dilithium;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Kyber;
using Org.BouncyCastle.Pqc.Crypto.SphincsPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.BouncyCastle.crypto.parameters
{
    public class HybridKeyParameters
        : AsymmetricKeyParameter
    {
        public static readonly Dictionary<string, string> HybridNameToOid = new Dictionary<string, string>()
        {
            { "p256_kyber512" , "1.3.9999.99.72" },
            { "x25519_kyber512" , "1.3.9999.99.49" },
            { "p384_kyber768" , "1.3.9999.99.73" },
            { "x448_kyber768" , "1.3.9999.99.50" },
            { "x25519_kyber768" , "1.3.9999.99.51" },
            { "p256_kyber768" , "1.3.9999.99.52" },
            { "p521_kyber1024" , "1.3.9999.99.74" },
            { "p256_mlkem512" , "1.3.6.1.4.1.22554.5.7.1" },
            { "x25519_mlkem512" , "1.3.6.1.4.1.22554.5.8.1" },
            { "p384_mlkem768" , "1.3.9999.99.75" },
            { "x448_mlkem768" , "1.3.9999.99.53" },
            { "x25519_mlkem768" , "1.3.9999.99.54" },
            { "p256_mlkem768" , "1.3.9999.99.55" },
            { "p521_mlkem1024" , "1.3.9999.99.76" },
            { "p384_mlkem1024" , "1.3.6.1.4.1.42235.6" },
            { "p256_dilithium2" , "1.3.9999.2.7.1" },
            //{ "rsa3072_dilithium2" , "1.3.9999.2.7.2" },
            { "p384_dilithium3" , "1.3.9999.2.7.3" },
            { "p521_dilithium5" , "1.3.9999.2.7.4" },
            { "p256_mldsa44" , "1.3.9999.7.1" },
            //{ "rsa3072_mldsa44" , "1.3.9999.7.2" },
            //{ "mldsa44_rsa2048" , "2.16.840.1.114027.80.8.1.2" },
            { "ed25519_mldsa44" , "2.16.840.1.114027.80.8.1.3" },
            { "p256_mldsa44" , "2.16.840.1.114027.80.8.1.4" },
            { "p384_mldsa65" , "1.3.9999.7.3" },
            //{ "mldsa65_rsa3072" , "2.16.840.1.114027.80.8.1.7" },
            { "p256_mldsa65" , "2.16.840.1.114027.80.8.1.8" },
            { "ed25519_mldsa65" , "2.16.840.1.114027.80.8.1.10" },
            { "p521_mldsa87" , "1.3.9999.7.4" },
            { "p384_mldsa87" , "2.16.840.1.114027.80.8.1.11" },
            { "ed448_mldsa87" , "2.16.840.1.114027.80.8.1.13" },
            { "p256_sphincssha2128fsimple" , "1.3.9999.6.4.14" },
            //{ "rsa3072_sphincssha2128fsimple" , "1.3.9999.6.4.15" },
            { "p256_sphincssha2128ssimple" , "1.3.9999.6.4.17" },
            //{ "rsa3072_sphincssha2128ssimple" , "1.3.9999.6.4.18" },
            { "p384_sphincssha2192fsimple" , "1.3.9999.6.5.11" },
            { "p384_sphincssha2192ssimple" , "1.3.9999.6.5.13" },
            { "p521_sphincssha2256fsimple" , "1.3.9999.6.6.11" },
            { "p521_sphincssha2256ssimple" , "1.3.9999.6.6.13" },
            { "p256_sphincsshake128fsimple" , "1.3.9999.6.7.14" },
            //{ "rsa3072_sphincsshake128fsimple" , "1.3.9999.6.7.15" },
            { "p256_sphincsshake128ssimple" , "1.3.9999.6.7.17" },
            //{ "rsa3072_sphincsshake128ssimple" , "1.3.9999.6.7.18" },
            { "p384_sphincsshake192fsimple" , "1.3.9999.6.8.11" },
            { "p384_sphincsshake192ssimple" , "1.3.9999.6.8.13" },
            { "p521_sphincsshake256fsimple" , "1.3.9999.6.9.11" },
            { "p521_sphincsshake256ssimple" , "1.3.9999.6.9.13" },
        };

        public static readonly Dictionary<string, string> HybridOidToName = new Dictionary<string, string>()
        {
            { "1.3.9999.99.72" , "p256_kyber512" },
            { "1.3.9999.99.49" , "x25519_kyber512" },
            { "1.3.9999.99.73" , "p384_kyber768" },
            { "1.3.9999.99.50" , "x448_kyber768" },
            { "1.3.9999.99.51" , "x25519_kyber768" },
            { "1.3.9999.99.52" , "p256_kyber768" },
            { "1.3.9999.99.74" , "p521_kyber1024" },
            { "1.3.6.1.4.1.22554.5.7.1" , "p256_mlkem512" },
            { "1.3.6.1.4.1.22554.5.8.1" , "x25519_mlkem512" },
            { "1.3.9999.99.75" , "p384_mlkem768" },
            { "1.3.9999.99.53" , "x448_mlkem768" },
            { "1.3.9999.99.54" , "x25519_mlkem768" },
            { "1.3.9999.99.55" , "p256_mlkem768" },
            { "1.3.9999.99.76" , "p521_mlkem1024" },
            { "1.3.6.1.4.1.42235.6" , "p384_mlkem1024" },
            { "1.3.9999.2.7.1" , "p256_dilithium2" },
            //{ "1.3.9999.2.7.2" , "rsa3072_dilithium2" },
            { "1.3.9999.2.7.3" , "p384_dilithium3" },
            { "1.3.9999.2.7.4" , "p521_dilithium5" },
            { "1.3.9999.7.1" , "p256_mldsa44" },
            //{ "1.3.9999.7.2" , "rsa3072_mldsa44" },
            //{ "2.16.840.1.114027.80.8.1.2" , "mldsa44_rsa2048" },
            { "2.16.840.1.114027.80.8.1.3" , "ed25519_mldsa44" },
            { "2.16.840.1.114027.80.8.1.4" , "p256_mldsa44" },
            { "1.3.9999.7.3" , "p384_mldsa65" },
            //{ "2.16.840.1.114027.80.8.1.7" , "mldsa65_rsa3072" },
            { "2.16.840.1.114027.80.8.1.8" , "p256_mldsa65" },
            { "2.16.840.1.114027.80.8.1.10" , "ed25519_mldsa65" },
            { "1.3.9999.7.4" , "p521_mldsa87" },
            { "2.16.840.1.114027.80.8.1.11" , "p384_mldsa87" },
            { "2.16.840.1.114027.80.8.1.13" , "ed448_mldsa87" },
            { "1.3.9999.6.4.14" , "p256_sphincssha2128fsimple" },
            //{ "1.3.9999.6.4.15" , "rsa3072_sphincssha2128fsimple" },
            { "1.3.9999.6.4.17" , "p256_sphincssha2128ssimple" },
            //{ "1.3.9999.6.4.18" , "rsa3072_sphincssha2128ssimple" },
            { "1.3.9999.6.5.11" , "p384_sphincssha2192fsimple" },
            { "1.3.9999.6.5.13" , "p384_sphincssha2192ssimple" },
            { "1.3.9999.6.6.11" , "p521_sphincssha2256fsimple" },
            { "1.3.9999.6.6.13" , "p521_sphincssha2256ssimple" },
            { "1.3.9999.6.7.14" , "p256_sphincsshake128fsimple" },
            //{ "1.3.9999.6.7.15" , "rsa3072_sphincsshake128fsimple" },
            { "1.3.9999.6.7.17" , "p256_sphincsshake128ssimple" },
            //{ "1.3.9999.6.7.18" , "rsa3072_sphincsshake128ssimple" },
            { "1.3.9999.6.8.11" , "p384_sphincsshake192fsimple" },
            { "1.3.9999.6.8.13" , "p384_sphincsshake192ssimple" },
            { "1.3.9999.6.9.11" , "p521_sphincsshake256fsimple" },
            { "1.3.9999.6.9.13" , "p521_sphincsshake256ssimple" },
        };

        public readonly DerObjectIdentifier AlgorithmOid;

        public AsymmetricKeyParameter Classical {  get; private set; }

        public AsymmetricKeyParameter PostQuantum {  get; private set; }

        public string CanonicalName { get; private set; }

        public HybridKeyParameters(AsymmetricKeyParameter classical, AsymmetricKeyParameter postQuantum, DerObjectIdentifier oid = null)
            : base(classical.IsPrivate)
        {
            if (classical.IsPrivate != postQuantum.IsPrivate)
                throw new ArgumentException("Mixed private and public keys");

            Classical = classical;
            PostQuantum = postQuantum;
            AlgorithmOid = null;

            if (oid != null)
            {
                AlgorithmOid = oid;
                CanonicalName = null;
                return;
            }

            string classicalCanonicalName = null;

            //if (Classical is RsaKeyParameters)
            //{
                //classicalCanonicalName = string.Concat("rsa", (Classical as RsaKeyParameters).Modulus.BitLength);
            //}
            if (Classical is ECKeyParameters)
            {
                var curve = (Classical as ECKeyParameters).Parameters.Curve;

                if (curve is SecP256R1Curve)
                {
                    classicalCanonicalName = "p256";
                }
                else if (curve is SecP384R1Curve)
                {
                    classicalCanonicalName = "p384";
                }
                else if (curve is SecP521R1Curve)
                {
                    classicalCanonicalName = "p521";
                }
            }
            else if (Classical is X25519PrivateKeyParameters || Classical is X25519PublicKeyParameters)
            {
                classicalCanonicalName = "x25519";
            }
            else if (Classical is Ed25519PrivateKeyParameters || Classical is Ed25519PublicKeyParameters)
            {
                classicalCanonicalName = "ed25519";
            }
            else if (Classical is X448PrivateKeyParameters || Classical is X448PublicKeyParameters)
            {
                classicalCanonicalName = "x448";
            }
            else if (Classical is Ed448PrivateKeyParameters || Classical is Ed448PrivateKeyParameters)
            {
                classicalCanonicalName = "ed448";
            }

            string postQuantumCanonicalName = null;

            if (PostQuantum is KyberKeyParameters)
            {
                postQuantumCanonicalName = (PostQuantum as KyberKeyParameters).Parameters.Name;
            }
            else if (PostQuantum is DilithiumKeyParameters)
            {
                postQuantumCanonicalName = string.Concat("dilithium", (PostQuantum as DilithiumKeyParameters).Parameters.GetEngine(null).Mode.ToString());
            }
            else if (PostQuantum is SphincsPlusKeyParameters)
            {
                postQuantumCanonicalName = (PostQuantum as SphincsPlusKeyParameters).Parameters.Name.Replace("-", "");
            }

            if (postQuantumCanonicalName != null && classicalCanonicalName != null)
            {
                CanonicalName = string.Concat(classicalCanonicalName, "_", postQuantumCanonicalName);
            }
            else
            {
                throw new Exception("Unsupported hybrid combination");
            }

            string objectId = null;
            if (CanonicalName != null)
            {
                if(!HybridNameToOid.TryGetValue(CanonicalName, out objectId))
                {
                    throw new Exception("Object identifier for hybrid combination not found");
                }
            }

            AlgorithmOid = new DerObjectIdentifier(objectId);
        }

        public override bool Equals(object obj)
        {
            return (this == obj) ||
                (obj is HybridKeyParameters other && IsPrivate == other.IsPrivate &&
                Classical.Equals(other.Classical) && PostQuantum.Equals(other.PostQuantum));
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + Classical.GetHashCode();
            hash = hash * 23 + PostQuantum.GetHashCode();
            return hash;
        }
    }
}
