export interface RecruitmentDetails {
  id: number,
  title: string,
  description: string,
  startDate: Date,
  type: number,
  jobType: string,
  jobLevel: number
}

export function newRecruitmentDetails(): RecruitmentDetails {
  return {
    id: 0,
    title: "",
    description: "",
    startDate: new Date(),
    type: 0,
    jobType: "",
    jobLevel: 0
  };
}
