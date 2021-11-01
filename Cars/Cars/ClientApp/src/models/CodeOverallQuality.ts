export interface CodeOverallQuality {
  id: number,
  success: boolean,

  completedTime: Date,

  codeSmells: number,
  maintainability: number,
  coverage: number,
  cognitiveComplexity: number,
  violations: number,
  securityRating: number,
  duplicatedLines: number,
  lines: number,
  duplicatedLinesDensity: number,
  bugs: number,
  sqaleRating: number,
  reliabilityRating: number,
  complexity: number,
  securityHotspots: number,

  overallRating: number,
}
