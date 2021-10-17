export interface ApplicationDto {
  recruitmentId: number,
  description: string,
  cvFile: string,
  clFile: string,
  projects: Array<string>,
  clauseRequiredAccepted,
  clauseOptAccepted,
  clauseOpt2Accepted
}

export const ApplicationDtoDefault: ApplicationDto = {

  recruitmentId: 0,
  description: "",
  cvFile: "",
  clFile: "",
  projects: new Array<string>(),
  clauseRequiredAccepted: false,
  clauseOpt2Accepted: false,
  clauseOptAccepted: false
}
