import {Applicant} from "./Applicant";
import {ProjectDto} from "./ProjectDto";
import {CodeOverallQuality} from "./CodeOverallQuality";

export interface RecruitmentApplication {
  id: number,

  applicant: Applicant,
  description: string,
  cvFile: string,
  clFile: string,
  projects: ProjectDto[],
  time: Date,
  clauseOptAccepted: boolean,
  clauseOpt2Accepted: boolean,
  codeOverallQuality: CodeOverallQuality,
  selected: boolean
}
