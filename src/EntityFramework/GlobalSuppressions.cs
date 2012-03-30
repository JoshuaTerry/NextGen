// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "System.Data.Entity.Migrations.Sql")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "System.Data.Entity.ModelConfiguration")]
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "System.Data.Entity.Validation")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "System.Data.Entity.Migrations.Utilities")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "System.Data.Entity.Migrations.History")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "System.Data.Entity.Migrations.Builders")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2243:AttributeStringLiteralsShouldParseCorrectly")]
[assembly:
    SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope = "member",
        Target = "System.Data.Entity.ModelConfiguration.Conventions.Sets.V1ConventionSet.#.cctor()")]
[assembly:
    SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member",
        Target =
            "System.Data.Entity.ModelConfiguration.Conventions.ForeignKeyDiscoveryConvention.#System.Data.Entity.ModelConfiguration.Conventions.IEdmConvention`1<System.Data.Entity.Edm.EdmAssociationType>.Apply(System.Data.Entity.Edm.EdmAssociationType,System.Data.Entity.Edm.EdmModel)"
        )]
[assembly:
    SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member",
        Target = "System.Data.Entity.Edm.Validation.Internal.EdmModel.EdmModelSyntacticValidationRules.#.cctor()")]
[assembly:
    SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope = "member",
        Target = "System.Data.Entity.Edm.Validation.Internal.EdmModel.EdmModelSemanticValidationRules.#.cctor()")]
[assembly:
    SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member",
        Target = "System.Data.Entity.Edm.Validation.Internal.EdmModel.EdmModelSemanticValidationRules.#.cctor()")]
[assembly:
    SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Scope = "member",
        Target = "System.Data.Entity.Edm.Validation.Internal.EdmModel.EdmModelSemanticValidationRules.#.cctor()")]
