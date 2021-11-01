import {CodeQualityAssessment} from "./CodeQualityAssessment";

export interface ProjectDto {
  title: string,
  description: string,
  url: string
  codeQualityAssessment: CodeQualityAssessment
}


