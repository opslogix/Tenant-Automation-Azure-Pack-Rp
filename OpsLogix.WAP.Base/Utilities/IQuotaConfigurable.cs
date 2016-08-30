//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace OpsLogix.WAP.Base.DataContracts
{
    /// <summary>
    /// Interface for entities that can be configured.
    /// </summary>
    public interface IQuotaConfigurable
    {
        /// <summary>
        /// Gets the state of the configuration.
        /// </summary>
        QuotaConfigurationState ConfigState { get; }
    }
}
