export interface CodeQualityAssessment {
  id: number,
  success: boolean,
  completedTime: Date,
  errors: number,
  security,
  codeSmells,
  duplications: number,
  readability,
  maintainability,
  technicalDebt
}
