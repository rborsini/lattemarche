import { AttachmentCategory } from './attachmentCategory.model';

export class Attachment {
    public Id: number = 0;
    public OriginalFileName: string = "";
    public RepositoryPath: string = "";
    public RepositoryFileName: string = "";
    public Size: number = 0;
    public MimeType: string = "";
    public ReferenceId: string = "";
    public ReferenceType: string = "";
    public Description: string = "";
    public CategoryId: number = 0;
    public Category_Description: string = "";
}