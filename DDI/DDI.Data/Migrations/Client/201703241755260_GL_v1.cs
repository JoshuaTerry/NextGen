namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GL_v1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PaymentMethodConstituents", newName: "ConstituentPaymentMethods");
            DropPrimaryKey("dbo.ConstituentPaymentMethods");
            CreateTable(
                "dbo.BusinessUnit",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 128),
                        BusinessUnitType = c.Int(nullable: false),
                        Code = c.String(maxLength: 16),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.AccountBudget",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountId = c.Guid(),
                        BudgetType = c.Int(nullable: false),
                        YearAmount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount01 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount02 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount03 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount04 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount05 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount06 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount07 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount08 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount09 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount10 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount11 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount12 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount13 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount14 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount01 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount02 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount03 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount04 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount05 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount06 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount07 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount08 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount09 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount10 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount11 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount12 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount13 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount14 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .Index(t => new { t.AccountId, t.BudgetType }, unique: true, name: "IX_Account_BudgetType");
            
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        AccountNumber = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        BeginningBalance = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Category = c.Int(nullable: false),
                        IsNormallyDebit = c.Boolean(nullable: false),
                        SortKey = c.String(maxLength: 128),
                        ClosingAccountId = c.Guid(),
                        Group1Id = c.Guid(),
                        Group2Id = c.Guid(),
                        Group3Id = c.Guid(),
                        Group4Id = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.ClosingAccountId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.AccountGroup", t => t.Group1Id)
                .ForeignKey("dbo.AccountGroup", t => t.Group2Id)
                .ForeignKey("dbo.AccountGroup", t => t.Group3Id)
                .ForeignKey("dbo.AccountGroup", t => t.Group4Id)
                .Index(t => t.FiscalYearId)
                .Index(t => t.SortKey)
                .Index(t => t.ClosingAccountId)
                .Index(t => t.Group1Id)
                .Index(t => t.Group2Id)
                .Index(t => t.Group3Id)
                .Index(t => t.Group4Id);
            
            CreateTable(
                "dbo.AccountSegment",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Level = c.Int(nullable: false),
                        AccountId = c.Guid(),
                        SegmentId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .ForeignKey("dbo.Segment", t => t.SegmentId)
                .Index(t => t.AccountId)
                .Index(t => t.SegmentId);
            
            CreateTable(
                "dbo.Segment",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        SegmentLevelId = c.Guid(),
                        Level = c.Int(nullable: false),
                        Code = c.String(maxLength: 30),
                        Name = c.String(maxLength: 128),
                        ParentSegmentId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Segment", t => t.ParentSegmentId)
                .ForeignKey("dbo.SegmentLevel", t => t.SegmentLevelId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .Index(t => new { t.FiscalYearId, t.SegmentLevelId, t.Code }, unique: true, name: "IX_Code")
                .Index(t => new { t.FiscalYearId, t.SegmentLevelId, t.Name }, unique: true, name: "IX_Name")
                .Index(t => t.ParentSegmentId);
            
            CreateTable(
                "dbo.FiscalYear",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LedgerId = c.Guid(),
                        Name = c.String(maxLength: 16),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        Status = c.Int(nullable: false),
                        NumberOfPeriods = c.Int(nullable: false),
                        CurrentPeriodNumber = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                        Ledger_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ledger", t => t.Ledger_Id)
                .ForeignKey("dbo.Ledger", t => t.LedgerId)
                .Index(t => new { t.LedgerId, t.Name }, unique: true, name: "IX_Name")
                .Index(t => t.Ledger_Id);
            
            CreateTable(
                "dbo.FiscalPeriod",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        PeriodNumber = c.Int(nullable: false),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        IsAdjustmentPeriod = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .Index(t => new { t.FiscalYearId, t.PeriodNumber }, unique: true, name: "IX_FiscalYear_PeriodNumber");
            
            CreateTable(
                "dbo.Ledger",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DefaultFiscalYearId = c.Guid(),
                        IsParent = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        NumberOfSegments = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        FixedBudgetName = c.String(maxLength: 40),
                        WorkingBudgetName = c.String(maxLength: 40),
                        WhatIfBudgetName = c.String(maxLength: 40),
                        ApproveJournals = c.Boolean(nullable: false),
                        FundAccounting = c.Boolean(nullable: false),
                        BusinessUnitId = c.Guid(),
                        PostAutomatically = c.Boolean(nullable: false),
                        OrgLedgerId = c.Guid(),
                        PriorPeriodPostingMode = c.Int(nullable: false),
                        CapitalizeHeaders = c.Boolean(nullable: false),
                        CopyCOAChanges = c.Boolean(nullable: false),
                        AccountGroupLevels = c.Int(nullable: false),
                        AccountGroup1Title = c.String(maxLength: 40),
                        AccountGroup2Title = c.String(maxLength: 40),
                        AccountGroup3Title = c.String(maxLength: 40),
                        AccountGroup4Title = c.String(maxLength: 40),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnitId)
                .ForeignKey("dbo.FiscalYear", t => t.DefaultFiscalYearId)
                .ForeignKey("dbo.Ledger", t => t.OrgLedgerId)
                .Index(t => t.DefaultFiscalYearId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.BusinessUnitId)
                .Index(t => t.OrgLedgerId);
            
            CreateTable(
                "dbo.LedgerAccount",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LedgerId = c.Guid(),
                        AccountNumber = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ledger", t => t.LedgerId)
                .Index(t => t.LedgerId);
            
            CreateTable(
                "dbo.LedgerAccountYear",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LedgerAccountId = c.Guid(),
                        FiscalYearId = c.Guid(),
                        AccountId = c.Guid(),
                        IsMerge = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccount", t => t.LedgerAccountId)
                .Index(t => t.LedgerAccountId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.SegmentLevel",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LedgerId = c.Guid(),
                        Level = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Format = c.Int(nullable: false),
                        Length = c.Int(nullable: false),
                        IsLinked = c.Boolean(nullable: false),
                        IsCommon = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 40),
                        Abbreviation = c.String(maxLength: 16),
                        Separator = c.String(maxLength: 1),
                        SortOrder = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ledger", t => t.LedgerId)
                .Index(t => new { t.LedgerId, t.Level }, unique: true, name: "IX_Level")
                .Index(t => new { t.LedgerId, t.Name }, unique: true, name: "IX_Name");
            
            CreateTable(
                "dbo.AccountGroup",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 128),
                        Sequence = c.Int(),
                        FiscalYearId = c.Guid(),
                        Category = c.Int(nullable: false),
                        ParentGroupId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountGroup", t => t.ParentGroupId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.ParentGroupId);
            
            CreateTable(
                "dbo.AccountPriorYear",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountId = c.Guid(),
                        PriorAccountId = c.Guid(),
                        Percentage = c.Decimal(nullable: false, precision: 5, scale: 2),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .ForeignKey("dbo.Account", t => t.PriorAccountId)
                .Index(t => t.AccountId)
                .Index(t => t.PriorAccountId);
            
            CreateTable(
                "dbo.AccountClose",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountId = c.Guid(),
                        Debit_Amount01 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount02 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount03 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount04 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount05 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount06 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount07 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount08 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount09 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount10 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount11 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount12 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount13 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount14 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount01 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount02 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount03 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount04 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount05 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount06 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount07 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount08 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount09 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount10 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount11 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount12 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount13 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount14 = c.Decimal(nullable: false, precision: 14, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.BusinessUnitFromTo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        BusinessUnitId = c.Guid(),
                        OffsettingBusinessUnitId = c.Guid(),
                        FromAccountId = c.Guid(),
                        ToAccountId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnitId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccount", t => t.FromAccountId)
                .ForeignKey("dbo.Fund", t => t.OffsettingBusinessUnitId)
                .ForeignKey("dbo.LedgerAccount", t => t.ToAccountId)
                .Index(t => new { t.FiscalYearId, t.BusinessUnitId, t.OffsettingBusinessUnitId }, unique: true, name: "IX_FiscalYear_BUs")
                .Index(t => t.FromAccountId)
                .Index(t => t.ToAccountId);
            
            CreateTable(
                "dbo.Fund",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        FundSegmentId = c.Guid(),
                        FundBalanceAccountId = c.Guid(),
                        ClosingRevenueAccountId = c.Guid(),
                        ClosingExpenseAccountId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LedgerAccount", t => t.ClosingExpenseAccountId)
                .ForeignKey("dbo.LedgerAccount", t => t.ClosingRevenueAccountId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccount", t => t.FundBalanceAccountId)
                .ForeignKey("dbo.Segment", t => t.FundSegmentId)
                .Index(t => new { t.FiscalYearId, t.FundSegmentId }, unique: true, name: "IX_FiscalYear_FundSegment")
                .Index(t => t.FundBalanceAccountId)
                .Index(t => t.ClosingRevenueAccountId)
                .Index(t => t.ClosingExpenseAccountId);
            
            CreateTable(
                "dbo.FundFromTo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        FundId = c.Guid(),
                        OffsettingFundId = c.Guid(),
                        FromAccountId = c.Guid(),
                        ToAccountId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                        Fund_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccount", t => t.FromAccountId)
                .ForeignKey("dbo.Fund", t => t.FundId)
                .ForeignKey("dbo.Fund", t => t.OffsettingFundId)
                .ForeignKey("dbo.LedgerAccount", t => t.ToAccountId)
                .ForeignKey("dbo.Fund", t => t.Fund_Id)
                .Index(t => new { t.FiscalYearId, t.FundId, t.OffsettingFundId }, unique: true, name: "IX_FiscalYear_Funds")
                .Index(t => t.FromAccountId)
                .Index(t => t.ToAccountId)
                .Index(t => t.Fund_Id);
            
            CreateTable(
                "dbo.LedgerAccountMerge",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FromAccountId = c.Guid(),
                        ToAccountId = c.Guid(),
                        FromAccountNumber = c.String(maxLength: 128),
                        ToAccountNumber = c.String(maxLength: 128),
                        FiscalYearId = c.Guid(),
                        MergedById = c.Guid(),
                        MergedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccount", t => t.FromAccountId)
                .ForeignKey("dbo.Users", t => t.MergedById)
                .ForeignKey("dbo.LedgerAccount", t => t.ToAccountId)
                .Index(t => t.FromAccountId)
                .Index(t => t.ToAccountId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.MergedById);
            
            CreateTable(
                "dbo.PostedTransaction",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TransactionNumber = c.Long(nullable: false),
                        LedgerAccountYearId = c.Guid(),
                        FiscalYearId = c.Guid(),
                        PeriodNumber = c.Int(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        TransactionDate = c.DateTime(storeType: "date"),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        DocumentType = c.String(maxLength: 4),
                        LineNumber = c.Int(nullable: false),
                        Description = c.String(maxLength: 255),
                        TransactionId = c.Int(nullable: false),
                        IsAdjustment = c.Boolean(nullable: false),
                        SubledgerTransactionId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccountYear", t => t.LedgerAccountYearId)
                .ForeignKey("dbo.SubledgerTransaction", t => t.SubledgerTransactionId)
                .Index(t => t.LedgerAccountYearId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.SubledgerTransactionId);
            
            CreateTable(
                "dbo.SubledgerTransaction",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        TransactionNumber = c.Long(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                        TransactionDate = c.DateTime(storeType: "date"),
                        PostDate = c.DateTime(),
                        DocumentType = c.String(maxLength: 4),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        DebitAccountId = c.Guid(),
                        CreditAccountId = c.Guid(),
                        Status = c.Int(nullable: false),
                        IsAdjustment = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LedgerAccountYear", t => t.CreditAccountId)
                .ForeignKey("dbo.LedgerAccountYear", t => t.DebitAccountId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.DebitAccountId)
                .Index(t => t.CreditAccountId);
            
            AddColumn("dbo.Users", "DefaultBusinessUnitId", c => c.Guid());
            AlterColumn("dbo.Address", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Address", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ConstituentAddress", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ConstituentAddress", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.AddressType", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.AddressType", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Constituent", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Constituent", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.AlternateId", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.AlternateId", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ClergyStatus", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ClergyStatus", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ClergyType", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ClergyType", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ConstituentStatus", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ConstituentStatus", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ConstituentType", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ConstituentType", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Tag", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Tag", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.TagGroup", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.TagGroup", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ContactInfo", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ContactInfo", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ContactType", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ContactType", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ContactCategory", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ContactCategory", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Denomination", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Denomination", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.DoingBusinessAs", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.DoingBusinessAs", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.EducationLevel", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.EducationLevel", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Education", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Education", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Degree", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Degree", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.School", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.School", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Ethnicity", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Ethnicity", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Gender", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Gender", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.IncomeLevel", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.IncomeLevel", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Language", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Language", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.MaritialStatus", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.MaritialStatus", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.PaymentMethod", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.PaymentMethod", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.EFTFormat", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.EFTFormat", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Prefix", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Prefix", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Profession", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Profession", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Relationship", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Relationship", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.RelationshipType", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.RelationshipType", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.RelationshipCategory", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.RelationshipCategory", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Region", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Region", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.RegionArea", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.RegionArea", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Users", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Users", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserClaims", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserClaims", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserLogins", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserLogins", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserRoles", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserRoles", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Configuration", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Configuration", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ConstituentPicture", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ConstituentPicture", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.FileStorage", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.FileStorage", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.CustomField", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.CustomField", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.CustomFieldData", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.CustomFieldData", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.CustomFieldOption", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.CustomFieldOption", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.NoteCategory", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.NoteCategory", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Note", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Note", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.NoteContactMethod", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.NoteContactMethod", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.NoteCode", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.NoteCode", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.NoteTopic", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.NoteTopic", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.RegionLevel", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.RegionLevel", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Roles", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Roles", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.SectionPreference", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.SectionPreference", "LastModifiedBy", c => c.String(maxLength: 64));
            AddPrimaryKey("dbo.ConstituentPaymentMethods", new[] { "Constituent_Id", "PaymentMethod_Id" });
            CreateIndex("dbo.Users", "DefaultBusinessUnitId");
            AddForeignKey("dbo.Users", "DefaultBusinessUnitId", "dbo.BusinessUnit", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostedTransaction", "SubledgerTransactionId", "dbo.SubledgerTransaction");
            DropForeignKey("dbo.SubledgerTransaction", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.SubledgerTransaction", "DebitAccountId", "dbo.LedgerAccountYear");
            DropForeignKey("dbo.SubledgerTransaction", "CreditAccountId", "dbo.LedgerAccountYear");
            DropForeignKey("dbo.PostedTransaction", "LedgerAccountYearId", "dbo.LedgerAccountYear");
            DropForeignKey("dbo.PostedTransaction", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.LedgerAccountMerge", "ToAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.LedgerAccountMerge", "MergedById", "dbo.Users");
            DropForeignKey("dbo.LedgerAccountMerge", "FromAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.LedgerAccountMerge", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.BusinessUnitFromTo", "ToAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.BusinessUnitFromTo", "OffsettingBusinessUnitId", "dbo.Fund");
            DropForeignKey("dbo.FundFromTo", "Fund_Id", "dbo.Fund");
            DropForeignKey("dbo.FundFromTo", "ToAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.FundFromTo", "OffsettingFundId", "dbo.Fund");
            DropForeignKey("dbo.FundFromTo", "FundId", "dbo.Fund");
            DropForeignKey("dbo.FundFromTo", "FromAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.FundFromTo", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Fund", "FundSegmentId", "dbo.Segment");
            DropForeignKey("dbo.Fund", "FundBalanceAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.Fund", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Fund", "ClosingRevenueAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.Fund", "ClosingExpenseAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.BusinessUnitFromTo", "FromAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.BusinessUnitFromTo", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.BusinessUnitFromTo", "BusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.AccountClose", "AccountId", "dbo.Account");
            DropForeignKey("dbo.AccountPriorYear", "PriorAccountId", "dbo.Account");
            DropForeignKey("dbo.AccountPriorYear", "AccountId", "dbo.Account");
            DropForeignKey("dbo.Account", "Group4Id", "dbo.AccountGroup");
            DropForeignKey("dbo.Account", "Group3Id", "dbo.AccountGroup");
            DropForeignKey("dbo.Account", "Group2Id", "dbo.AccountGroup");
            DropForeignKey("dbo.Account", "Group1Id", "dbo.AccountGroup");
            DropForeignKey("dbo.AccountGroup", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.AccountGroup", "ParentGroupId", "dbo.AccountGroup");
            DropForeignKey("dbo.Account", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Account", "ClosingAccountId", "dbo.Account");
            DropForeignKey("dbo.AccountBudget", "AccountId", "dbo.Account");
            DropForeignKey("dbo.Segment", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.FiscalYear", "LedgerId", "dbo.Ledger");
            DropForeignKey("dbo.Segment", "SegmentLevelId", "dbo.SegmentLevel");
            DropForeignKey("dbo.SegmentLevel", "LedgerId", "dbo.Ledger");
            DropForeignKey("dbo.Ledger", "OrgLedgerId", "dbo.Ledger");
            DropForeignKey("dbo.LedgerAccountYear", "LedgerAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.LedgerAccountYear", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.LedgerAccountYear", "AccountId", "dbo.Account");
            DropForeignKey("dbo.LedgerAccount", "LedgerId", "dbo.Ledger");
            DropForeignKey("dbo.FiscalYear", "Ledger_Id", "dbo.Ledger");
            DropForeignKey("dbo.Ledger", "DefaultFiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Ledger", "BusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.FiscalPeriod", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Segment", "ParentSegmentId", "dbo.Segment");
            DropForeignKey("dbo.AccountSegment", "SegmentId", "dbo.Segment");
            DropForeignKey("dbo.AccountSegment", "AccountId", "dbo.Account");
            DropForeignKey("dbo.Users", "DefaultBusinessUnitId", "dbo.BusinessUnit");
            DropIndex("dbo.SubledgerTransaction", new[] { "CreditAccountId" });
            DropIndex("dbo.SubledgerTransaction", new[] { "DebitAccountId" });
            DropIndex("dbo.SubledgerTransaction", new[] { "FiscalYearId" });
            DropIndex("dbo.PostedTransaction", new[] { "SubledgerTransactionId" });
            DropIndex("dbo.PostedTransaction", new[] { "FiscalYearId" });
            DropIndex("dbo.PostedTransaction", new[] { "LedgerAccountYearId" });
            DropIndex("dbo.LedgerAccountMerge", new[] { "MergedById" });
            DropIndex("dbo.LedgerAccountMerge", new[] { "FiscalYearId" });
            DropIndex("dbo.LedgerAccountMerge", new[] { "ToAccountId" });
            DropIndex("dbo.LedgerAccountMerge", new[] { "FromAccountId" });
            DropIndex("dbo.FundFromTo", new[] { "Fund_Id" });
            DropIndex("dbo.FundFromTo", new[] { "ToAccountId" });
            DropIndex("dbo.FundFromTo", new[] { "FromAccountId" });
            DropIndex("dbo.FundFromTo", "IX_FiscalYear_Funds");
            DropIndex("dbo.Fund", new[] { "ClosingExpenseAccountId" });
            DropIndex("dbo.Fund", new[] { "ClosingRevenueAccountId" });
            DropIndex("dbo.Fund", new[] { "FundBalanceAccountId" });
            DropIndex("dbo.Fund", "IX_FiscalYear_FundSegment");
            DropIndex("dbo.BusinessUnitFromTo", new[] { "ToAccountId" });
            DropIndex("dbo.BusinessUnitFromTo", new[] { "FromAccountId" });
            DropIndex("dbo.BusinessUnitFromTo", "IX_FiscalYear_BUs");
            DropIndex("dbo.AccountClose", new[] { "AccountId" });
            DropIndex("dbo.AccountPriorYear", new[] { "PriorAccountId" });
            DropIndex("dbo.AccountPriorYear", new[] { "AccountId" });
            DropIndex("dbo.AccountGroup", new[] { "ParentGroupId" });
            DropIndex("dbo.AccountGroup", new[] { "FiscalYearId" });
            DropIndex("dbo.SegmentLevel", "IX_Name");
            DropIndex("dbo.SegmentLevel", "IX_Level");
            DropIndex("dbo.LedgerAccountYear", new[] { "AccountId" });
            DropIndex("dbo.LedgerAccountYear", new[] { "FiscalYearId" });
            DropIndex("dbo.LedgerAccountYear", new[] { "LedgerAccountId" });
            DropIndex("dbo.LedgerAccount", new[] { "LedgerId" });
            DropIndex("dbo.Ledger", new[] { "OrgLedgerId" });
            DropIndex("dbo.Ledger", new[] { "BusinessUnitId" });
            DropIndex("dbo.Ledger", new[] { "Name" });
            DropIndex("dbo.Ledger", new[] { "Code" });
            DropIndex("dbo.Ledger", new[] { "DefaultFiscalYearId" });
            DropIndex("dbo.FiscalPeriod", "IX_FiscalYear_PeriodNumber");
            DropIndex("dbo.FiscalYear", new[] { "Ledger_Id" });
            DropIndex("dbo.FiscalYear", "IX_Name");
            DropIndex("dbo.Segment", new[] { "ParentSegmentId" });
            DropIndex("dbo.Segment", "IX_Name");
            DropIndex("dbo.Segment", "IX_Code");
            DropIndex("dbo.AccountSegment", new[] { "SegmentId" });
            DropIndex("dbo.AccountSegment", new[] { "AccountId" });
            DropIndex("dbo.Account", new[] { "Group4Id" });
            DropIndex("dbo.Account", new[] { "Group3Id" });
            DropIndex("dbo.Account", new[] { "Group2Id" });
            DropIndex("dbo.Account", new[] { "Group1Id" });
            DropIndex("dbo.Account", new[] { "ClosingAccountId" });
            DropIndex("dbo.Account", new[] { "SortKey" });
            DropIndex("dbo.Account", new[] { "FiscalYearId" });
            DropIndex("dbo.AccountBudget", "IX_Account_BudgetType");
            DropIndex("dbo.BusinessUnit", new[] { "Code" });
            DropIndex("dbo.BusinessUnit", new[] { "Name" });
            DropIndex("dbo.Users", new[] { "DefaultBusinessUnitId" });
            DropPrimaryKey("dbo.ConstituentPaymentMethods");
            AlterColumn("dbo.SectionPreference", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.SectionPreference", "CreatedBy", c => c.String());
            AlterColumn("dbo.Roles", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Roles", "CreatedBy", c => c.String());
            AlterColumn("dbo.RegionLevel", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.RegionLevel", "CreatedBy", c => c.String());
            AlterColumn("dbo.NoteTopic", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.NoteTopic", "CreatedBy", c => c.String());
            AlterColumn("dbo.NoteCode", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.NoteCode", "CreatedBy", c => c.String());
            AlterColumn("dbo.NoteContactMethod", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.NoteContactMethod", "CreatedBy", c => c.String());
            AlterColumn("dbo.Note", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Note", "CreatedBy", c => c.String());
            AlterColumn("dbo.NoteCategory", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.NoteCategory", "CreatedBy", c => c.String());
            AlterColumn("dbo.CustomFieldOption", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.CustomFieldOption", "CreatedBy", c => c.String());
            AlterColumn("dbo.CustomFieldData", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.CustomFieldData", "CreatedBy", c => c.String());
            AlterColumn("dbo.CustomField", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.CustomField", "CreatedBy", c => c.String());
            AlterColumn("dbo.FileStorage", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.FileStorage", "CreatedBy", c => c.String());
            AlterColumn("dbo.ConstituentPicture", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ConstituentPicture", "CreatedBy", c => c.String());
            AlterColumn("dbo.Configuration", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Configuration", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserRoles", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserRoles", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserLogins", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserLogins", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserClaims", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserClaims", "CreatedBy", c => c.String());
            AlterColumn("dbo.Users", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Users", "CreatedBy", c => c.String());
            AlterColumn("dbo.RegionArea", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.RegionArea", "CreatedBy", c => c.String());
            AlterColumn("dbo.Region", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Region", "CreatedBy", c => c.String());
            AlterColumn("dbo.RelationshipCategory", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.RelationshipCategory", "CreatedBy", c => c.String());
            AlterColumn("dbo.RelationshipType", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.RelationshipType", "CreatedBy", c => c.String());
            AlterColumn("dbo.Relationship", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Relationship", "CreatedBy", c => c.String());
            AlterColumn("dbo.Profession", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Profession", "CreatedBy", c => c.String());
            AlterColumn("dbo.Prefix", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Prefix", "CreatedBy", c => c.String());
            AlterColumn("dbo.EFTFormat", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.EFTFormat", "CreatedBy", c => c.String());
            AlterColumn("dbo.PaymentMethod", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.PaymentMethod", "CreatedBy", c => c.String());
            AlterColumn("dbo.MaritialStatus", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.MaritialStatus", "CreatedBy", c => c.String());
            AlterColumn("dbo.Language", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Language", "CreatedBy", c => c.String());
            AlterColumn("dbo.IncomeLevel", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.IncomeLevel", "CreatedBy", c => c.String());
            AlterColumn("dbo.Gender", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Gender", "CreatedBy", c => c.String());
            AlterColumn("dbo.Ethnicity", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Ethnicity", "CreatedBy", c => c.String());
            AlterColumn("dbo.School", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.School", "CreatedBy", c => c.String());
            AlterColumn("dbo.Degree", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Degree", "CreatedBy", c => c.String());
            AlterColumn("dbo.Education", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Education", "CreatedBy", c => c.String());
            AlterColumn("dbo.EducationLevel", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.EducationLevel", "CreatedBy", c => c.String());
            AlterColumn("dbo.DoingBusinessAs", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.DoingBusinessAs", "CreatedBy", c => c.String());
            AlterColumn("dbo.Denomination", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Denomination", "CreatedBy", c => c.String());
            AlterColumn("dbo.ContactCategory", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ContactCategory", "CreatedBy", c => c.String());
            AlterColumn("dbo.ContactType", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ContactType", "CreatedBy", c => c.String());
            AlterColumn("dbo.ContactInfo", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ContactInfo", "CreatedBy", c => c.String());
            AlterColumn("dbo.TagGroup", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.TagGroup", "CreatedBy", c => c.String());
            AlterColumn("dbo.Tag", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Tag", "CreatedBy", c => c.String());
            AlterColumn("dbo.ConstituentType", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ConstituentType", "CreatedBy", c => c.String());
            AlterColumn("dbo.ConstituentStatus", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ConstituentStatus", "CreatedBy", c => c.String());
            AlterColumn("dbo.ClergyType", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ClergyType", "CreatedBy", c => c.String());
            AlterColumn("dbo.ClergyStatus", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ClergyStatus", "CreatedBy", c => c.String());
            AlterColumn("dbo.AlternateId", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.AlternateId", "CreatedBy", c => c.String());
            AlterColumn("dbo.Constituent", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Constituent", "CreatedBy", c => c.String());
            AlterColumn("dbo.AddressType", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.AddressType", "CreatedBy", c => c.String());
            AlterColumn("dbo.ConstituentAddress", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ConstituentAddress", "CreatedBy", c => c.String());
            AlterColumn("dbo.Address", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Address", "CreatedBy", c => c.String());
            DropColumn("dbo.Users", "DefaultBusinessUnitId");
            DropTable("dbo.SubledgerTransaction");
            DropTable("dbo.PostedTransaction");
            DropTable("dbo.LedgerAccountMerge");
            DropTable("dbo.FundFromTo");
            DropTable("dbo.Fund");
            DropTable("dbo.BusinessUnitFromTo");
            DropTable("dbo.AccountClose");
            DropTable("dbo.AccountPriorYear");
            DropTable("dbo.AccountGroup");
            DropTable("dbo.SegmentLevel");
            DropTable("dbo.LedgerAccountYear");
            DropTable("dbo.LedgerAccount");
            DropTable("dbo.Ledger");
            DropTable("dbo.FiscalPeriod");
            DropTable("dbo.FiscalYear");
            DropTable("dbo.Segment");
            DropTable("dbo.AccountSegment");
            DropTable("dbo.Account");
            DropTable("dbo.AccountBudget");
            DropTable("dbo.BusinessUnit");
            AddPrimaryKey("dbo.ConstituentPaymentMethods", new[] { "PaymentMethod_Id", "Constituent_Id" });
            RenameTable(name: "dbo.ConstituentPaymentMethods", newName: "PaymentMethodConstituents");
        }
    }
}
