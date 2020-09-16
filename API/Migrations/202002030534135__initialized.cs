namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _initialized : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalesInvoice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceNumber = c.Int(nullable: false),
                        InvoiceDate = c.DateTime(nullable: false),
                        CustomerCode = c.String(),
                        CustomerName = c.String(),
                        ItemCode = c.String(),
                        ItemDescription = c.String(),
                        UOM = c.String(),
                        Quantity = c.Double(nullable: false),
                        Rate = c.Double(nullable: false),
                        TaxCode = c.String(),
                        TaxPercentage = c.Double(nullable: false),
                        TotalTaxAmount = c.Double(nullable: false),
                        TotalAmountBfTax = c.Double(nullable: false),
                        TotalAmountAfTax = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SalesInvoice");
        }
    }
}
