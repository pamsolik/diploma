import {JobLevel} from "./JobLevel";
import {JobType} from "./JobType";
import {RecruitmentDetailsDto} from "../RecruitmentDetailsDto";
import {getEnumKeyByEnumValue} from "../../components/EnumTool";
import {RecruitmentStatus} from "./RecruitmentStatus";
import {RecruitmentType} from "./RecruitmentType";
import {TeamSize} from "./TeamSize";

export class RecruitmentEnums{
  public jobLevels: string[] = Object.values(JobLevel);
  jobLevel: string;

  jobTypes: string[] = Object.values(JobType);
  jobType: string;

  recruitmentStatuses: string[] = Object.values(RecruitmentStatus);
  recruitmentStatus: string;

  recruitmentTypes: string[] = Object.values(RecruitmentType);
  recruitmentType: string;

  teamSizes: string[] = Object.values(TeamSize);
  teamSize: string;

  public updateRecruitmentSettings(settings: RecruitmentDetailsDto){
    settings.jobLevel = getEnumKeyByEnumValue(JobLevel, this.jobLevel);
    settings.jobType = getEnumKeyByEnumValue(JobType, this.jobType);
    settings.status = getEnumKeyByEnumValue(RecruitmentStatus, this.recruitmentStatus);
    settings.type = getEnumKeyByEnumValue(RecruitmentType, this.recruitmentType);
    settings.teamSize = getEnumKeyByEnumValue(TeamSize, this.teamSize);
  }
}
