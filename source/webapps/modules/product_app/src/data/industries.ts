
// this enum is controlled by Sopheon.CloudNative.Products.DataAccess.SeedData.ProductSeedData.cs
// (EnumAttributeOptions defined for the Industry EnumCollectionAttribute)
// keep this TS enum in sync with changes to the C# file ProductSeedData.cs

// eslint-disable-next-line no-shadow
export enum Industries {
  Advertising = -1,
  AgricultureAndForestry = -2,
  Construction = -3,
  EducationHigherEd = -4,
  EducationK12 = -5,
  EnergyMiningOilAndGas = -6,
  FinancialServices = -7,
  GovernmentFederal = -8,
  GovernmentLocal = -9,
  GovernmentMilitary = -10,
  GovernmentState = -11,
  HealthCare = -12,
  Insurance = -13,
  ManufacturingAerospace = -14,
  ManufacturingAutomotive = -15,
  ManufacturingConsumerGoods = -16,
  ManufacturingIndustrial = -17,
  MediaAndEntertainment = -18,
  MembershipOrganizations = -19,
  NonProfit = -20,
  PharmaceuticalsAndBiotech = -21,
  ProfessionalAndTechnicalServices = -22,
  RealEstateRentalAndLeasing = -23,
  Retail = -24,
  TechnologyHardware = -25,
  TechnologySoftwareAndServices = -26,
  Telecommunications = -27,
  TransportationAndWarehousing = -28,
  TravelLeisureAndHospitality = -29,
  Utilities = -30
}

export interface IndustryUxMapItem {
  key: number;
  iconName: string;
  resourceKey: string;
}

export const industriesUxMap: IndustryUxMapItem[] = [
  { key: Industries.Advertising, resourceKey: 'industryoption.advertising', iconName: 'MediaIndustryIcon' },
  { key: Industries.AgricultureAndForestry, resourceKey: 'industryoption.agricuture', iconName: 'AgIndustryIcon' },
  { key: Industries.Construction, resourceKey: 'industryoption.construction', iconName: 'ConstREIndustryIcon' },
  { key: Industries.EducationHigherEd, resourceKey: 'industryoption.eduhigher', iconName: 'EduIndustryIcon' },
  { key: Industries.EducationK12, resourceKey: 'industryoption.eduk12', iconName: 'EduIndustryIcon' },
  { key: Industries.EnergyMiningOilAndGas, resourceKey: 'industryoption.energy', iconName: 'EnergyIndustryIcon' },
  { key: Industries.FinancialServices, resourceKey: 'industryoption.financialservices', iconName: 'FinIndustryIcon' },
  { key: Industries.GovernmentFederal, resourceKey: 'industryoption.govfederal', iconName: 'GovtIndustryIcon' },
  { key: Industries.GovernmentLocal, resourceKey: 'industryoption.govlocal', iconName: 'GovtIndustryIcon' },
  { key: Industries.GovernmentMilitary, resourceKey: 'industryoption.govmilitary', iconName: 'GovtIndustryIcon' },
  { key: Industries.GovernmentState, resourceKey: 'industryoption.govstate', iconName: 'GovtIndustryIcon' },
  { key: Industries.HealthCare, resourceKey: 'industryoption.healthcare', iconName: 'HealthIndustryIcon' },
  { key: Industries.Insurance, resourceKey: 'industryoption.insurance', iconName: 'FinIndustryIcon' },
  { key: Industries.ManufacturingAerospace, resourceKey: 'industryoption.manuaero', iconName: 'AeroIndustryIcon' },
  { key: Industries.ManufacturingAutomotive, resourceKey: 'industryoption.manuauto', iconName: 'AutoIndustryIcon' },
  { key: Industries.ManufacturingConsumerGoods, resourceKey: 'industryoption.manuconsumergoods', iconName: 'ConsumerIndustryIcon' },
  { key: Industries.ManufacturingIndustrial, resourceKey: 'industryoption.manuindustrial', iconName: 'IndusIndustryIcon' },
  { key: Industries.MediaAndEntertainment, resourceKey: 'industryoption.entertainment', iconName: 'MediaIndustryIcon' },
  { key: Industries.MembershipOrganizations, resourceKey: 'industryoption.membershiporg', iconName: 'MemberIndustryIcon' },
  { key: Industries.NonProfit, resourceKey: 'industryoption.nonprofit', iconName: 'MemberIndustryIcon' },
  { key: Industries.PharmaceuticalsAndBiotech, resourceKey: 'industryoption.pharma', iconName: 'HealthIndustryIcon' },
  { key: Industries.ProfessionalAndTechnicalServices, resourceKey: 'industryoption.protechservices', iconName: 'ServicesIndustryIcon' },
  { key: Industries.RealEstateRentalAndLeasing, resourceKey: 'industryoption.realestate', iconName: 'ConstREIndustryIcon' },
  { key: Industries.Retail, resourceKey: 'industryoption.retail', iconName: 'ConsumerIndustryIcon' },
  { key: Industries.TechnologyHardware, resourceKey: 'industryoption.techhardware', iconName: 'TechIndustryIcon' },
  { key: Industries.TechnologySoftwareAndServices, resourceKey: 'industryoption.techsoftware', iconName: 'TechIndustryIcon' },
  { key: Industries.Telecommunications, resourceKey: 'industryoption.telecom', iconName: 'TeleIndustryIcon' },
  { key: Industries.TransportationAndWarehousing, resourceKey: 'industryoption.transportation', iconName: 'TransIndustryIcon' },
  { key: Industries.TravelLeisureAndHospitality, resourceKey: 'industryoption.travel', iconName: 'HospIndustryIcon' },
  { key: Industries.Utilities, resourceKey: 'industryoption.utilities', iconName: 'TechIndustryIcon' },
];

