import {Applicant} from "./Applicant";
import {CodeQualityAssessment} from "./CodeQualityAssessment";

export interface RecruitmentApplication {
  id: number,
  // recruitment: number,
  applicant: Applicant,
  description: string,
  cvFile: string,
  clFile: string,
  projects: [],
  time: Date,
  clauseOptAccepted: boolean,
  clauseOpt2Accepted: boolean,
  codeQualityAssessment: CodeQualityAssessment
}
