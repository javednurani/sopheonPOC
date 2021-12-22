// this enum is controlled by Sopheon.CloudNative.Products.DataAccess.SeedData.ProductSeedData.cs
// (EnumAttributeOptions defined for the Status EnumAttribute)
// keep this TS enum in sync with changes to the C# file ProductSeedData.cs

// eslint-disable-next-line no-shadow
export enum Status { // TODO Cloud-2183 story, set these values to 1/2/3/4 after Cloud-2466 service layer work
  NotStarted = 1,
  InProgress = 2,
  Assigned = 3,
  Complete = 4,
}
