import {City} from "./City";

export interface RecruitmentDetailsView {
  id: number,
  title: string,
  shortDescription: string,
  description: string,
  status: number,
  type: number,
  jobType: number,
  jobLevel: number,
  teamSize: number,
  field: string,
  city: City,
  latitude: number,
  longitude: number,
  imgUrl: string,
  startDate: Date,
  clauseRequired: string,
  clauseOpt1: string,
  clauseOpt2: string
}
