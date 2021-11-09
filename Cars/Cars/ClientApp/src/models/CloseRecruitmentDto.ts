export interface CloseRecruitmentDto {
  recruitmentId: number;
  recruitmentsToClose: RecruitmentToClose[];
}

export interface RecruitmentToClose {
  applicationId: number;
  selected: boolean;
}
