//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OpsLogix.WAP.Base.DataContracts
{
    /// <summary>
    /// A list of subscription certificates
    /// </summary>
    [CollectionDataContract(Namespace = "http://schemas.microsoft.com/windowsazure", Name = "SubscriptionCertificates", ItemName = "SubscriptionCertificate")]
    public class SubscriptionCertificateList : List<SubscriptionCertificate>
    {
    }
}
