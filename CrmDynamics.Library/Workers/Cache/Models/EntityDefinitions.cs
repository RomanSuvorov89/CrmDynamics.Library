using CrmDynamics.Library.Workers.Cache.Models.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrmDynamics.Library.Workers.Cache.Models
{
    public class EntityDefinitions
    {
        [DataMember(Name = "@odata.context")]
        public string Context { get; set; }
        public int ActivityTypeMask { get; set; }
        public bool AutoRouteToOwnerQueue { get; set; }
        public bool CanTriggerWorkflow { get; set; }
        public LocalizedLabelsProperty Description { get; set; }
        public LocalizedLabelsProperty DisplayCollectionName { get; set; }
        public LocalizedLabelsProperty DisplayName { get; set; }
        public bool EntityHelpUrlEnabled { get; set; }
        public string EntityHelpUrl { get; set; }
        public bool IsDocumentManagementEnabled { get; set; }
        public bool IsOneNoteIntegrationEnabled { get; set; }
        public bool IsInteractionCentricEnabled { get; set; }
        public bool IsKnowledgeManagementEnabled { get; set; }
        public bool IsSLAEnabled { get; set; }
        public bool IsBPFEntity { get; set; }
        public bool IsDocumentRecommendationsEnabled { get; set; }
        public bool AutoCreateAccessTeams { get; set; }
        public bool IsActivity { get; set; }
        public bool IsActivityParty { get; set; }
        public CanBeChangedProperty IsAuditEnabled { get; set; }
        public bool IsAvailableOffline { get; set; }
        public bool IsChildEntity { get; set; }
        public bool IsAIRUpdated { get; set; }
        public CanBeChangedProperty IsValidForQueue { get; set; }
        public CanBeChangedProperty IsConnectionsEnabled { get; set; }
        public object IconLargeName { get; set; }
        public object IconMediumName { get; set; }
        public object IconSmallName { get; set; }
        public bool IsCustomEntity { get; set; }
        public bool IsBusinessProcessEnabled { get; set; }
        public CanBeChangedProperty IsCustomizable { get; set; }
        public CanBeChangedProperty IsRenameable { get; set; }
        public CanBeChangedProperty IsMappable { get; set; }
        public CanBeChangedProperty IsDuplicateDetectionEnabled { get; set; }
        public CanBeChangedProperty CanCreateAttributes { get; set; }
        public CanBeChangedProperty CanCreateForms { get; set; }
        public CanBeChangedProperty CanCreateViews { get; set; }
        public CanBeChangedProperty CanCreateCharts { get; set; }
        public CanBeChangedProperty CanBeRelatedEntityInRelationship { get; set; }
        public CanBeChangedProperty CanBePrimaryEntityInRelationship { get; set; }
        public CanBeChangedProperty CanBeInManyToMany { get; set; }
        public CanBeChangedProperty CanEnableSyncToExternalSearchIndex { get; set; }
        public bool SyncToExternalSearchIndex { get; set; }
        public CanBeChangedProperty CanModifyAdditionalSettings { get; set; }
        public CanBeChangedProperty CanChangeHierarchicalRelationship { get; set; }
        public bool IsOptimisticConcurrencyEnabled { get; set; }
        public bool ChangeTrackingEnabled { get; set; }
        public CanBeChangedProperty CanChangeTrackingBeEnabled { get; set; }
        public bool IsImportable { get; set; }
        public bool IsIntersect { get; set; }
        public CanBeChangedProperty IsMailMergeEnabled { get; set; }
        public bool IsManaged { get; set; }
        public bool IsEnabledForCharts { get; set; }
        public bool IsEnabledForTrace { get; set; }
        public bool IsValidForAdvancedFind { get; set; }
        public CanBeChangedProperty IsVisibleInMobile { get; set; }
        public CanBeChangedProperty IsVisibleInMobileClient { get; set; }
        public CanBeChangedProperty IsReadOnlyInMobileClient { get; set; }
        public CanBeChangedProperty IsOfflineInMobileClient { get; set; }
        public int DaysSinceRecordLastModified { get; set; }
        public string MobileOfflineFilters { get; set; }
        public bool IsReadingPaneEnabled { get; set; }
        public bool IsQuickCreateEnabled { get; set; }
        public string LogicalName { get; set; }
        public int ObjectTypeCode { get; set; }
        public string OwnershipType { get; set; }
        public string PrimaryNameAttribute { get; set; }
        public string PrimaryImageAttribute { get; set; }
        public string PrimaryIdAttribute { get; set; }
        public IList<Privilege> Privileges { get; set; }
        public object RecurrenceBaseEntityLogicalName { get; set; }
        public string ReportViewName { get; set; }
        public string SchemaName { get; set; }
        public string IntroducedVersion { get; set; }
        public bool IsStateModelAware { get; set; }
        public bool EnforceStateTransitions { get; set; }
        public string EntityColor { get; set; }
        public string LogicalCollectionName { get; set; }
        public string CollectionSchemaName { get; set; }
        public string EntitySetName { get; set; }
        public bool IsEnabledForExternalChannels { get; set; }
        public bool IsPrivate { get; set; }
        public bool UsesBusinessDataLabelTable { get; set; }
        public bool IsLogicalEntity { get; set; }
        public string MetadataId { get; set; }
        public object HasChanged { get; set; }
    }

    public class Privilege
    {
        public bool CanBeBasic { get; set; }
        public bool CanBeDeep { get; set; }
        public bool CanBeGlobal { get; set; }
        public bool CanBeLocal { get; set; }
        public bool CanBeEntityReference { get; set; }
        public bool CanBeParentEntityReference { get; set; }
        public string Name { get; set; }
        public string PrivilegeId { get; set; }
        public string PrivilegeType { get; set; }
    }
}
