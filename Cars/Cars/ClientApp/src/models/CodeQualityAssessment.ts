import {CodeOverallQuality} from "./CodeOverallQuality";

export interface CodeQualityAssessment extends CodeOverallQuality {
  ProjectsCount: number,
}
