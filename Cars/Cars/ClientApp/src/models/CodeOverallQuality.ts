export interface CodeOverallQuality {
  id: number,
  success: boolean,
  retries: number,
  completedTime: Date,

  complexity: number,
  cognitiveComplexity: number,

  duplicatedLines: number,
  duplicatedLinesDensity: number,

  violations: number,

  codeSmells: number,
  technicalDebt: number,
  maintainabilityRating: number,

  bugs: number,
  reliabilityRating: number,

  coverage: number,
  tests: number,

  securityRating: number,
  securityHotspots: number,

  linesOfCode: number,

  overallRating: number,
}
