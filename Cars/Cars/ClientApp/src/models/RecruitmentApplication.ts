import {Applicant} from "./Applicant";
import {CodeQualityAssessment} from "./CodeQualityAssessment";
import {ProjectDto} from "./ProjectDto";

export interface RecruitmentApplication {
  id: number,
  // recruitment: number,
  applicant: Applicant,
  description: string,
  cvFile: string,
  clFile: string,
  projects: ProjectDto[],
  time: Date,
  clauseOptAccepted: boolean,
  clauseOpt2Accepted: boolean,
  codeQualityAssessment: CodeQualityAssessment,
  selected: boolean
}
