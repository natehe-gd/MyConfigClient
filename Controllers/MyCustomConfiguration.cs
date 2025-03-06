public class MyCustomConfiguration
{
    public string Priority { get; set; }
    public MCSendProcessorData[] MCSendProcessor { get; set; }
    public VisaDirectProcessorData[] VisaDirectProcessor { get; set; }
    public BaaSSettingsData[] BaaSSettings { get; set; }
    public NecSettingsData[] NecSettings { get; set; }
    public Level1Data[] Level1 { get; set; }

    public class MCSendProcessorData
    {
        public string PartnerId { get; set; }
        public string PaymentType { get; set; }
        public string[] TransferType { get; set; }
        public string CardAccepterId { get; set; }
        public string Statementdescriptor { get; set; }
    }

    public class VisaDirectProcessorData
    {
        public string TransactionDescription { get; set; }
        public string[] TransferType { get; set; }
        public string PanEntryMode { get; set; }
        public string PosConditionCode { get; set; }
        public string MotoECIIndicator { get; set; }
        public string CardAcceptorCountry { get; set; }
        public string CardAcceptorZipCode { get; set; }
        public string CardAcceptorCounty { get; set; }
        public string CardAcceptorState { get; set; }
        public string IdCode { get; set; }
        public string Name { get; set; }
        public string TerminalId { get; set; }
        public string MerchantCategoryCode { get; set; }
        public string MerchantPseudoAbaNumber { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderCity { get; set; }
        public string SenderStateCode { get; set; }
        public string SenderPostalCode { get; set; }
        public string SenderCountryCode { get; set; }
        public string SourceOfFundsCode { get; set; }
        public string AcquirerCountryCode { get; set; }
        public string AcquiringBin { get; set; }
        public string BusinessApplicationIdForDisbursement { get; set; }
        public string BusinessApplicationIdForA2A { get; set; }
        public string TransactionCurrencyCode { get; set; }
        public string MvvAcquirerAssigned { get; set; }
        public string MvvVisaAssigned { get; set; }
    }

    public class BaaSSettingsData
    {
        public string AdjustmentDescription { get; set; }
        public string[] TransferType { get; set; }
    }

    public class NecSettingsData
    {
        public string TransactionDescription { get; set; }
        public string[] TransferType { get; set; }
    }

    public class Level1Data
    {
        public Level2Data[] Level2 { get; set; }

        public class Level2Data
        {
            public string Level3 { get; set; }
        }
    }
}

