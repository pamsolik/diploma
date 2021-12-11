import {Component, Inject, Input} from '@angular/core';
import {NgbModal, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';
import {RecruitmentApplication} from "../../models/RecruitmentApplication";
import {Technology} from "../../models/enums/Technology";
import {ProjectDto} from "../../models/ProjectDto";
import {CodeOverallQuality} from "../../models/CodeOverallQuality";
import {formatNumber} from "../../util/Formatter";

@Component({
    selector: 'app-projects-details-details',
    templateUrl: './projects-details.component.html',
    styleUrls: ['./projects-details.component.scss'],
})
export class ProjectsDetailsComponent {

    @Input()
    details: RecruitmentApplication;
    technologies: string[] = Object.values(Technology);
    isCoq: boolean = false;
    codeQuality: CodeOverallQuality;
    project: ProjectDto;
    modalRef: NgbModalRef;

    constructor(private modalService: NgbModal,
                @Inject('BASE_URL') private baseUrl: string) {
    }

    openProject(content: any, project: number) {
        this.openModal(content);
        this.isCoq = false;
        this.project = this.details.projects[project];
        this.codeQuality = this.project.codeQualityAssessment;
    }

    openCoq(content: any) {
        this.openModal(content);
        this.isCoq = true;
        this.codeQuality = this.details.codeOverallQuality;
    }

    getValue(value: number): string {
        return value === null || value === undefined ? "N/A" : formatNumber(value).toString();
    }

    getMultiplied(value: number, multiplier: number): number {
        return value * multiplier;
    }

    openModal(content: any) {
        let settings = {
            ariaLabelledBy: 'modal-basic-title',
            centered: true,
            size: 'lg',
        }
        this.modalRef = this.modalService.open(content, settings);
        console.log(this.details);
    }

    secondsToHms(min: number): string {
        if (!min) return "N/A"
        let h = Math.floor(min / 60);
        let m = Math.floor(min - h * 60);
        let hDisplay = h > 0 ? h + "h, " : "";
        let mDisplay = m > 0 ? m + "min" : "";
        return hDisplay + mDisplay;
    }

    close() {
        this.modalRef.close();
    }
}
