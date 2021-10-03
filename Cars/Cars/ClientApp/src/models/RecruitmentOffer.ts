export interface RecruitmentList{
  items: RecruitmentOffer[],
  totalItems: number
}

export interface RecruitmentOffer {
  id: number,
  title: string,
  description: string,
  type: number,
  jobType: string,
  jobLevel: number,
  imgUrl: string
}
