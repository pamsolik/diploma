import {ProjectDto} from "./ProjectDto";

export interface ApplicationDto {
  recruitmentId: number,
  description: string,
  cvFile: string,
  clFile: string,
  projects: ProjectDto[],
  clauseRequiredAccepted,
  clauseOptAccepted,
  clauseOpt2Accepted
}

export const ApplicationDtoDefault: ApplicationDto = {
  recruitmentId: 0,
  description: "",
  cvFile: "",
  clFile: "",
  projects: [],
  clauseRequiredAccepted: false,
  clauseOpt2Accepted: false,
  clauseOptAccepted: false
}
