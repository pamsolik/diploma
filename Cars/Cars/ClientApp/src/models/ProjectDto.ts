import {CodeQualityAssessment} from "./CodeQualityAssessment";

export interface ProjectDto {
  title: string,
  description: string,
  url: string,
  technology: number,
  retries: number,
  solutionsCnt: number,
  codeQualityAssessment: CodeQualityAssessment
}


