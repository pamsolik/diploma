import {City} from "./City";

export class RecruitmentDetailsDto {
  title: string = "";
  shortDescription: string = "";
  description: string = "";
  status: number = 0;
  type: number = 0;
  jobType: number = 0;
  jobLevel: number = 0;
  teamSize: number = 0;
  field: string = "";
  city: City = new City();
  imgUrl: string = "";
  clauseRequired: string = "";
  clauseOpt1: string = "";
  clauseOpt2: string = "";
}
