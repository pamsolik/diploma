import {City} from "./City";

export interface RecruitmentList {
  items: RecruitmentOffer[],
  totalItems: number,
  pageIndex: number,
  pageSize: number
}

export interface RecruitmentOffer {
  id: number,
  title: string,
  shortDescription: string,
  type: number,
  jobType: string,
  jobLevel: number,
  imgUrl: string,
  startDate: Date,
  daysAgo: string,
  city: City,
}
