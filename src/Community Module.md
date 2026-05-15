# **Community Module Documentation**

---

## **1\. Overview**

The Community module is implemented across:

* `src/Presentation/Mentora.API/Controllers/Community/*` — community API endpoints  
* `src/Core/Mentora.Application/Services/Community/*` — community business logic  
* `src/Core/Mentora.Application/DTOs/Community/*` — request/response contracts  
* `src/Core/Mentora.Domain/Entities/*Community*.cs` — community data model  
* `src/Infrastructure/Mentora.Persistence/Repositories/Community/*` — data access  
* `ApiResponse.cs` — standard API response wrapper

The module covers:

* Community creation, update, delete  
* Membership management  
* Community posts, comments, likes, saves, shares  
* Reporting posts/comments

All routes are under `api/communities` and require authentication.

---

## **2\. Business Logic & Constraints**

* Community names must be unique globally.  
* A community creator is automatically added as `Owner`.  
* `CreateCommunity` requires `Name`, optional `Description`, optional `CoverImageUrl`, and `DomainId`.

**Ownership rules:**

* Only `Owner` or `Admin` can update community details.  
* Only `Owner` can delete the community.

**Member rules:**

* Users join via `/join`.  
* Banned members cannot join.  
* An `Owner` cannot leave their own community.

**Role rules:**

* Only the `Owner` can change roles.  
* `Owner` role cannot be assigned or reassigned through role update.  
* `Owner` cannot be demoted or removed.

**Ban rules:**

* Only `Owner` or `Admin` can ban members.  
* `Owner` cannot be banned.

**Post rules:**

* Only community members can create community posts.  
* Only post authors can update their post.  
* Posts can be deleted by the author, or by `Owner` / `Admin` of the community.

**Comment rules:**

* Any authenticated user can create comments on existing posts.  
* Only comment author can update comment.  
* A comment can be deleted by author or by community `Owner` / `Admin`.

**Like/Save/Share rules:**

* Likes and saves are toggle operations.  
* Share does not require membership check; it only requires the post exists.

**Report rules:**

* Any authenticated user can report a post and optionally a comment.  
* Report target comment must exist if `TargetCommentId` is provided.  
* Only `Owner` or `Admin` can update report status or delete reports.  
* Report list endpoint only returns pending reports for the given post.

---

## **3\. Roles & Permissions**

### **Roles**

From `CommunityRole.cs`:

| Value | Role |
| ----- | ----- |
| 1 | Owner |
| 2 | Admin |
| 3 | Member |

### **Permissions Table**

| Action | Owner | Admin | Member |
| ----- | ----- | ----- | ----- |
| Update community | ✅ | ✅ | ❌ |
| Delete community | ✅ | ❌ | ❌ |
| Change member roles | ✅ | ❌ | ❌ |
| Remove members | ✅ | ✅ | ❌ |
| Ban members | ✅ | ✅ | ❌ |
| Delete posts/comments/reports | ✅ | ✅ | ❌ |
| Create posts | ✅ | ✅ | ✅ |
| Comment | ✅ | ✅ | ✅ |
| Like/Save/Share posts | ✅ | ✅ | ✅ |
| Report content | ✅ | ✅ | ✅ |

---

## **4\. DTOs Documentation**

### **Community DTOs**

#### **`CreateCommunityDto`**

| Field | Type | Required | Nullable |
| ----- | ----- | ----- | ----- |
| Name | string | ✅ Yes | ❌ No |
| Description | string? | ❌ No | ✅ Yes |
| CoverImageUrl | string? | ❌ No | ✅ Yes |
| DomainId | int | ✅ Yes | ❌ No |

#### **`UpdateCommunityDto`**

| Field | Type | Required | Nullable |
| ----- | ----- | ----- | ----- |
| Name | string? | ❌ No | ✅ Yes |
| Description | string? | ❌ No | ✅ Yes |
| CoverImageUrl | string? | ❌ No | ✅ Yes |
| DomainId | int? | ❌ No | ✅ Yes |

#### **`CommunityResponseDto`**

| Field | Type | Nullable |
| ----- | ----- | ----- |
| CommunityId | Guid | ❌ No |
| Name | string | ❌ No |
| Description | string? | ✅ Yes |
| CoverImageUrl | string? | ✅ Yes |
| DomainId | int | ❌ No |
| MembersCount | int | ❌ No |
| PostsCount | int | ❌ No |
| CreatedAt | DateTime | ❌ No |
| CreatedByUserName | string | ❌ No |
| CreatedByUserProfilePicture | string? | ✅ Yes |

---

### **Post DTOs**

#### **`CreatePostDto`**

| Field | Type | Required | Nullable |
| ----- | ----- | ----- | ----- |
| Content | string | ✅ Yes | ❌ No |
| ImageUrl | string? | ❌ No | ✅ Yes |
| LinkUrl | string? | ❌ No | ✅ Yes |

#### **`UpdatePostDto`**

| Field | Type | Required | Nullable |
| ----- | ----- | ----- | ----- |
| Content | string? | ❌ No | ✅ Yes |
| ImageUrl | string? | ❌ No | ✅ Yes |
| LinkUrl | string? | ❌ No | ✅ Yes |

#### **`PostResponseDto`**

| Field | Type | Nullable |
| ----- | ----- | ----- |
| CommunityPostId | Guid | ❌ No |
| Content | string | ❌ No |
| ImageUrl | string? | ✅ Yes |
| LinkUrl | string? | ✅ Yes |
| CreatedAt | DateTime | ❌ No |
| LikesCount | int | ❌ No |
| CommentsCount | int | ❌ No |
| SharesCount | int | ❌ No |
| AuthorId | Guid | ❌ No |
| AuthorName | string | ❌ No |
| AuthorProfilePicture | string? | ✅ Yes |

---

### **Comment DTOs**

#### **`CreateCommentDto`**

| Field | Type | Required | Nullable |
| ----- | ----- | ----- | ----- |
| Content | string | ✅ Yes | ❌ No |

#### **`UpdateCommentDto`**

| Field | Type | Required | Nullable |
| ----- | ----- | ----- | ----- |
| Content | string | ✅ Yes | ❌ No |

#### **`CommentResponseDto`**

| Field | Type | Nullable |
| ----- | ----- | ----- |
| CommunityCommentId | Guid | ❌ No |
| Content | string | ❌ No |
| CreatedAt | DateTime | ❌ No |
| AuthorId | Guid | ❌ No |
| AuthorName | string | ❌ No |
| AuthorProfilePicture | string? | ✅ Yes |

---

### **Membership DTOs**

#### **`MemberResponseDto`**

| Field | Type | Nullable |
| ----- | ----- | ----- |
| UserId | Guid | ❌ No |
| UserName | string | ❌ No |
| ProfilePictureUrl | string? | ✅ Yes |
| Role | CommunityRole | ❌ No |
| JoinedAt | DateTime | ❌ No |

---

### **Report DTOs**

#### **`CreateReportDto`**

| Field | Type | Required | Nullable |
| ----- | ----- | ----- | ----- |
| ReportReason | CommunityReportReason | ✅ Yes | ❌ No |
| AdditionalComment | string? | ❌ No | ✅ Yes |
| TargetCommentId | Guid? | ❌ No | ✅ Yes |

#### **`ReportResponseDto`**

| Field | Type | Nullable |
| ----- | ----- | ----- |
| CommunityReportId | Guid | ❌ No |
| ReportReason | CommunityReportReason | ❌ No |
| AdditionalComment | string? | ✅ Yes |
| Status | CommunityReportStatus | ❌ No |
| CreatedAt | DateTime | ❌ No |
| ReporterUserId | Guid | ❌ No |
| ReporterName | string | ❌ No |
| TargetPostId | Guid? | ✅ Yes |
| TargetCommentId | Guid? | ✅ Yes |
| ReportsCount | int | ❌ No |

---

## **5\. APIs Documentation**

### **Community Endpoints**

#### **`POST`** /api/communities/create 

* **Description:** Create a new community  
* **Auth:** Required  
* **Body:** `CreateCommunityDto`  
* {  
*   "name": "hi com",  
*   "description": "A community for developers",  
*   "coverImageUrl": "https://img.com/cover.png",  
*   "domainId": 1  
* }  
*   
* **Response:** `ApiResponse<Guid>>`  
*   "success": **true**,  
*     "message": "Community created successfully",  
*     "data": "d1f9b96e-8b2f-4ece-a330-9fb04df2b33f",  
*     "errors": **null**  
* }  
*   
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`GET /api/communities/{communityId}`**

* **Description:** Get a single community by ID  
* **Auth:** Required  
* **Params:** `communityId` (Guid)  
* **Response:** `ApiResponse<CommunityResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Success",  
*     "data": {  
*         "communityId": "d1f9b96e-8b2f-4ece-a330-9fb04df2b33f",  
*         "name": "hi com",  
*         "description": "A community for developers",  
*         "coverImageUrl": "https://img.com/cover.png",  
*         "domainId": 1,  
*         "membersCount": 1,  
*         "postsCount": 0,  
*         "createdAt": "2026-05-12T18:25:23.9535221",  
*         "createdByUserName": "Rehab Baajar",  
*         "createdByUserProfilePicture": **null**  
*     },  
*     "errors": **null**  
* }  
*   
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`GET /api/communities/all`**

* **Description:** Get all communities  
* **Auth:** Required  
* **Response:** `ApiResponse<IEnumerable<CommunityResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Success",  
*     "data": \[  
*         {  
*             "communityId": "d1f9b96e-8b2f-4ece-a330-9fb04df2b33f",  
*             "name": "hi com",  
*             "description": "A community for developers",  
*             "coverImageUrl": "https://img.com/cover.png",  
*             "domainId": 1,  
*             "membersCount": 1,  
*             "postsCount": 0,  
*             "createdAt": "2026-05-12T18:25:23.9535221",  
*             "createdByUserName": "Rehab Baajar",  
*             "createdByUserProfilePicture": **null**  
*         },  
*         {  
*             "communityId": "cfa63294-a74f-4bf6-b187-b6084c305028",  
*             "name": "Tdev com",  
*             "description": "A community for developers",  
*             "coverImageUrl": "https://img.com/cover.png",  
*             "domainId": 1,  
*             "membersCount": 1,  
*             "postsCount": 0,  
*             "createdAt": "2026-05-12T17:58:34.4998289",  
*             "createdByUserName": "Rehab Baajar",  
*             "createdByUserProfilePicture": **null**  
*         },  
*         {  
*             "communityId": "ca68a38e-e231-4119-b311-e695b69d508b",  
*             "name": "Tech Hub",  
*             "description": "A community for developers",  
*             "coverImageUrl": "https://img.com/cover.png",  
*             "domainId": 1,  
*             "membersCount": 1,  
*             "postsCount": 0,  
*             "createdAt": "2026-05-12T17:58:08.4107921",  
*             "createdByUserName": "Rehab Baajar",  
*             "createdByUserProfilePicture": **null**  
*         }  
*     \],  
*     "errors": **null**  
* }  
*   
* `>>`  
* **Status Codes:** `200 OK`

#### **`GET /api/communities/my`**

* **Description:** Get communities the current user is a member of  
* **Auth:** Required  
* **Response:** `ApiResponse<IEnumerable<CommunityResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Success",  
*     "data": \[  
*         {  
*             "communityId": "cfa63294-a74f-4bf6-b187-b6084c305028",  
*             "name": "Tdev com",  
*             "description": "A community for developers",  
*             "coverImageUrl": "https://img.com/cover.png",  
*             "domainId": 1,  
*             "membersCount": 1,  
*             "postsCount": 0,  
*             "createdAt": "2026-05-12T17:58:34.4998289",  
*             "createdByUserName": "Rehab Baajar",  
*             "createdByUserProfilePicture": **null**  
*         },  
*         {  
*             "communityId": "d1f9b96e-8b2f-4ece-a330-9fb04df2b33f",  
*             "name": "hi com",  
*             "description": "A community for developers",  
*             "coverImageUrl": "https://img.com/cover.png",  
*             "domainId": 1,  
*             "membersCount": 1,  
*             "postsCount": 0,  
*             "createdAt": "2026-05-12T18:25:23.9535221",  
*             "createdByUserName": "Rehab Baajar",  
*             "createdByUserProfilePicture": **null**  
*         },  
*         {  
*             "communityId": "ca68a38e-e231-4119-b311-e695b69d508b",  
*             "name": "Tech Hub",  
*             "description": "A community for developers",  
*             "coverImageUrl": "https://img.com/cover.png",  
*             "domainId": 1,  
*             "membersCount": 1,  
*             "postsCount": 0,  
*             "createdAt": "2026-05-12T17:58:08.4107921",  
*             "createdByUserName": "Rehab Baajar",  
*             "createdByUserProfilePicture": **null**  
*         }  
*     \],  
*     "errors": **null**  
* }  
* `>>`  
* **Status Codes:** `200 OK`

#### **`PATCH /api/communities/{communityId}`**

* **Description:** Update community details (Owner or Admin only)  
* **Auth:** Required  
* **Params:** `communityId` (Guid)  
* **Body:** `UpdateCommunityDto`  
* {  
*   "name": "Ai com",  
*   "description": "string",  
*   "coverImageUrl": "string",  
*   "domainId": 2  
* }  
*   
* **Response:** `ApiResponse<bool`  
* {  
*     "success": **true**,  
*     "message": "Community updated successfully",  
*     "data": **true**,  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`DELETE /api/communities/{communityId}`**

* **Description:** Delete a community (Owner only)  
* **Auth:** Required  
* **Params:** `communityId` (Guid)  
* **Response:** `ApiResponse<bool`  
* {  
*     "success": **true**,  
*     "message": "Community deleted successfully",  
*     "data": **true**,  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`GET /api/communities/{communityId}/link`**

* **Description:** Get a generated shareable link for the community  
* **Auth:** Required  
* **Params:** `communityId` (Guid)  
* **Response:** `ApiResponse<string`  
* {  
*     "success": **true**,  
*     "message": "Community link generated",  
*     "data": "https://mentora.com/community/d1f9b96e-8b2f-4ece-a330-9fb04df2b33f",  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

---

### **Membership Endpoints**

#### **`POST /api/communities/{communityId}/join`**

* **Description:** Join a community  
* **Auth:** Required  
* **Params:** `communityId` (Guid)  
* **Response:** `ApiResponse<bool`  
* `if you are member`   
* {  
*     "success": **false**,  
*     "message": "You are already a member of this community",  
*     "data": **false**,  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`DELETE /api/communities/{communityId}/leave`**

* **Description:** Leave a community (Owner cannot leave)  
* **Auth:** Required  
* **Params:** `communityId` (Guid)  
* **Response:** `ApiResponse<bool`  
*  if you are owner you can leave the community  
* {  
*     "success": **false**,  
*     "message": "Owner cannot leave the community",  
*     "data": **false**,  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`DELETE /api/communities/{communityId}/members/{targetUserId}`**

* **Description:** Remove a member (Owner or Admin only)  
* **Auth:** Required  
* **Params:** `communityId` (Guid), `targetUserId` (Guid)  
* **Response:** `ApiResponse<bool>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`PATCH /api/communities/{communityId}/members/{targetUserId}/role`**

* **Description:** Change a member's role (Owner only, cannot assign Owner role)  
* **Auth:** Required  
* **Params:** `communityId` (Guid), `targetUserId` (Guid)  
* **Body:** `CommunityRole` (raw enum int, e.g. `2` for Admin)  
* **Response:** `ApiResponse<bool>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`POST /api/communities/{communityId}/members/{targetUserId}/ban`**

* **Description:** Ban a member (Owner or Admin only, cannot ban Owner)  
* **Auth:** Required  
* **Params:** `communityId` (Guid), `targetUserId` (Guid)  
* **Response:** `ApiResponse<bool>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`GET /api/communities/{communityId}/admins`**

* **Description:** Get list of admins in the community  
* **Auth:** Required  
* **Params:** `communityId` (Guid)  
* **Response:** `ApiResponse<IEnumerable<MemberResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Success",  
*     "data": \[\],  
*     "errors": **null**  
* }  
* `>>`  
* **Status Codes:** `200 OK`

#### **`GET /api/communities/{communityId}/members`**

* **Description:** Get list of all members in the community  
* **Auth:** Required  
* **Params:** `communityId` (Guid)  
* **Response:** `ApiResponse<IEnumerable<MemberResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Success",  
*     "data": \[\],  
*     "errors": **null**  
* }  
* `>>`  
* **Status Codes:** `200 OK`

---

### **Post Endpoints**

#### **`POST /api/communities/{communityId}/posts`**

* **Description:** Create a post in a community (members only)  
* **Auth:** Required  
* **Params:** `communityId` (Guid)  
* **Body:** `CreatePostDto`  
* {  
*   "content": "حد هنا يتعلم باك اند ي جماعه",  
*   "imageUrl": "string",  
*   "linkUrl": "string"  
* }  
*   
* **Response:** `ApiResponse<`  
* {  
*     "success": **true**,  
*     "message": "Post created successfully",  
*     "data": "437d08f1-cc14-4f7f-81ed-ba5449e5488c",  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`GET /api/communities/{communityId}/posts`**

* **Description:** Get all posts in a community  
* **Auth:** Required  
* **Params:** `communityId` (Guid)  
* **Response:** `ApiResponse<IEnumerable<PostResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Success",  
*     "data": \[  
*         {  
*             "communityPostId": "369b8a50-ffe6-4216-8e0d-15d3a49a787f",  
*             "content": "ok",  
*             "imageUrl": "string",  
*             "linkUrl": **null**,  
*             "createdAt": "2026-05-12T21:32:09.2978223",  
*             "likesCount": 0,  
*             "commentsCount": 0,  
*             "authorId": "0cc81526-226b-4136-95e1-5dcd964bbc68",  
*             "authorName": "Rehab Baajar",  
*             "authorProfilePicture": **null**,  
*             "comments": \[\]  
*         },  
*         {  
*             "communityPostId": "8b772237-ae10-4efa-bc88-e487c31aeb71",  
*             "content": "حد هنا يتعلم باك اند ي جماعه",  
*             "imageUrl": "string",  
*             "linkUrl": **null**,  
*             "createdAt": "2026-05-12T20:33:50.1687942",  
*             "likesCount": 0,  
*             "commentsCount": 0,  
*             "authorId": "0cc81526-226b-4136-95e1-5dcd964bbc68",  
*             "authorName": "Rehab Baajar",  
*             "authorProfilePicture": **null**,  
*             "comments": \[\]  
*         }  
*     \],  
*     "errors": **null**  
* }  
* `>>`  
* **Status Codes:** `200 OK`

#### **`GET /api/communities/posts/feed`**

* **Description:** Get feed of posts from all communities the user is a member of  
* **Auth:** Required  
* **Response:** `ApiResponse<IEnumerable<FeedResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Success",  
*     "data": \[  
*         {  
*             "communityPostId": "369b8a50-ffe6-4216-8e0d-15d3a49a787f",  
*             "content": "ok",  
*             "imageUrl": "string",  
*             "linkUrl": **null**,  
*             "createdAt": "2026-05-12T21:32:09.2978223",  
*             "likesCount": 0,  
*             "commentsCount": 0,  
*             "authorId": "00000000-0000-0000-0000-000000000000",  
*             "authorName": "Rehab Baajar",  
*             "authorProfilePicture": **null**  
*         },  
*         {  
*             "communityPostId": "8b772237-ae10-4efa-bc88-e487c31aeb71",  
*             "content": "حد هنا يتعلم باك اند ي جماعه",  
*             "imageUrl": "string",  
*             "linkUrl": **null**,  
*             "createdAt": "2026-05-12T20:33:50.1687942",  
*             "likesCount": 0,  
*             "commentsCount": 0,  
*             "authorId": "00000000-0000-0000-0000-000000000000",  
*             "authorName": "Rehab Baajar",  
*             "authorProfilePicture": **null**  
*         }  
*     \],  
*     "errors": **null**  
* }  
* `>>`  
* **Status Codes:** `200 OK`

#### **`GET /api/communities/posts/{postId}`**

* **Description:** Get a single post by ID  
* **Auth:** Required  
* **Params:** `postId` (Guid)  
* **Response:** `ApiResponse<PostResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Success",  
*     "data": {  
*         "communityPostId": "437d08f1-cc14-4f7f-81ed-ba5449e5488c",  
*         "content": "حد هنا يتعلم باك اند ي جماعه",  
*         "imageUrl": "string",  
*         "linkUrl": "string",  
*         "createdAt": "2026-05-12T20:37:19.3415815",  
*         "likesCount": 0,  
*         "commentsCount": 0,  
*         "sharesCount": 0,  
*         "authorId": "0cc81526-226b-4136-95e1-5dcd964bbc68",  
*         "authorName": "Rehab Baajar",  
*         "authorProfilePicture": **null**  
*     },  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`PATCH /api/communities/posts/{postId}`**

* **Description:** Update a post (author only/if you want update the post Content must not empty  
* {  
*     "success": **false**,  
*     "message": "Content cannot be empty",  
*     "data": **null**,  
*     "errors": **null**  
* }  
*  )  
* **Auth:** Required  
* **Params:** `postId` (Guid)  
* **Body:** `UpdatePostDto`  
* {  
*   "content": "gkgkgkkk",  
*   "imageUrl": "",  
*   "linkUrl": ""  
* }  
*   
* **Response:** `ApiResponse<PostResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Post updated successfully",  
*     "data": {  
*         "communityPostId": "437d08f1-cc14-4f7f-81ed-ba5449e5488c",  
*         "content": "gkgkgkkk",  
*         "imageUrl": "",  
*         "linkUrl": "",  
*         "createdAt": "2026-05-12T20:37:19.3415815",  
*         "likesCount": 0,  
*         "commentsCount": 0,  
*         "authorId": "0cc81526-226b-4136-95e1-5dcd964bbc68",  
*         "authorName": "Rehab Baajar",  
*         "authorProfilePicture": **null**,  
*         "comments": \[\]  
*     },  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`DELETE /api/communities/posts/{postId}`**

* **Description:** Delete a post (author, Owner, or Admin)  
* **Auth:** Required  
* **Params:** `postId` (Guid)  
* **Response:** `ApiResponse<bool`  
* {  
*     "success": **true**,  
*     "message": "Post deleted successfully",  
*     "data": **true**,  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

---

### **Comment Endpoints**

#### **`POST /api/communities/posts/{postId}/comments`**

* **Description:** Create a comment on a post (any authenticated user)  
* **Auth:** Required  
* **Params:** `postId` (Guid)  
* **Body:** `CreateCommentDto`  
* {  
*   "content": "ايوا اصاحبي انا اتفضل"  
* }  
*   
* **Response:** `ApiResponse<Guid>` (returns the new comment ID  
* {  
*     "success": **true**,  
*     "message": "Comment added successfully",  
*     "data": "3eba5590-624b-47d3-81d0-08deb06fcea4",  
*     "errors": **null**  
* }  
* )  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`GET /api/communities/posts/{postId}/comments`**

* **Description:** Get all comments on a post  
* **Auth:** Required  
* **Params:** `postId` (Guid)  
* **Response:** `ApiResponse<IEnumerable<CommentResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Success",  
*     "data": \[  
*         {  
*             "communityCommentId": "00b62602-1349-46de-81d1-08deb06fcea4",  
*             "content": "ايوا ازعيم  اومر",  
*             "createdAt": "2026-05-12T21:47:48.851173",  
*             "authorId": "0cc81526-226b-4136-95e1-5dcd964bbc68",  
*             "authorName": "Rehab Baajar",  
*             "authorProfilePicture": **null**  
*         },  
*         {  
*             "communityCommentId": "3eba5590-624b-47d3-81d0-08deb06fcea4",  
*             "content": "ايوا اصاحبي انا اتفضل",  
*             "createdAt": "2026-05-12T21:45:38.7683597",  
*             "authorId": "0cc81526-226b-4136-95e1-5dcd964bbc68",  
*             "authorName": "Rehab Baajar",  
*             "authorProfilePicture": **null**  
*         }  
*     \],  
*     "errors": **null**  
* }  
* `>>`  
* **Status Codes:** `200 OK`

#### **`PATCH /api/communities/posts/comments/{commentId}`**

* **Description:** Update a comment (author only)  
* **Auth:** Required  
* **Params:** `commentId` (Guid)  
* **Body:** `UpdateCommentDto`  
* {  
*   "content": "لا"  
* }  
*   
* **Response:** `ApiResponse<CommentResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Comment updated successfully",  
*     "data": {  
*         "communityCommentId": "3eba5590-624b-47d3-81d0-08deb06fcea4",  
*         "content": "لا",  
*         "createdAt": "2026-05-12T21:45:38.7683597",  
*         "authorId": "0cc81526-226b-4136-95e1-5dcd964bbc68",  
*         "authorName": "Rehab Baajar",  
*         "authorProfilePicture": **null**  
*     },  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`DELETE /api/communities/posts/comments/{commentId}`**

* **Description:** Delete a comment (author, Owner, or Admin)  
* **Auth:** Required  
* **Params:** `commentId` (Guid)  
* **Response:** `ApiResponse<bool`  
* {  
*     "success": **true**,  
*     "message": "Comment deleted successfully",  
*     "data": **true**,  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

---

### **Like Endpoints**

#### **`POST /api/communities/posts/{postId}/like`**

* **Description:** Toggle like on a post (like if not liked, unlike if already liked)  
* **Auth:** Required  
* **Params:** `postId` (Guid)  
* **Response:** `ApiResponse<bool`  
* {  
*     "success": **true**,  
*     "message": "Post liked",  
*     "data": **true**,  
*     "errors": **null**  
* }  
* {  
*     "success": **true**,  
*     "message": "Post unliked",  
*     "data": **false**,  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

---

### **Save Endpoints**

#### **`POST /api/communities/posts/{postId}/save`**

* **Description:** Toggle save on a post (save if not saved, unsave if already saved)  
* **Auth:** Required  
* **Params:** `postId` (Guid)  
* **Response:** `ApiResponse<bool`  
* {  
*     "success": **true**,  
*     "message": "Post saved",  
*     "data": **true**,  
*     "errors": **null**  
* }  
* {  
*     "success": **true**,  
*     "message": "Post unsaved",  
*     "data": **false**,  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`GET /api/communities/posts/saved`**

* **Description:** Get all posts saved by the current user  
* **Auth:** Required  
* **Response:** `ApiResponse<IEnumerable<PostResponseDto`  
* `>>`  
* **Status Codes:** `200 OK`

---

### **Share Endpoints**

#### **`POST /api/communities/posts/{postId}/share`**

* **Description:** Generate a shareable URL for a post (no membership check required)  
* **Auth:** Required  
* **Params:** `postId` (Guid)  
* **Response:** `ApiResponse<string>` (returns generated share URL  
* {  
*     "success": **true**,  
*     "message": "Post shared successfully",  
*     "data": "https://mentora.com/community/posts/8b772237-ae10-4efa-bc88-e487c31aeb71",  
*     "errors": **null**  
* }  
* )  
* **Status Codes:** `200 OK`, `400 Bad Request`

---

### **Report Endpoints**

#### **`POST /api/communities/{communityId}/posts/{postId}/report`**

* **Description:** Report a post, optionally targeting a specific comment  
* if report about comment   
* body  
* {  
*   "reportReason": 1,  
*   "additionalComment": "string",  
*   "targetCommentId": "00b62602-1349-46de-81d1-08deb06fcea4"  
* }  
*   
    
* **Auth:** Required  
* **Params:** `communityId` (Guid), `postId` (Guid)  
* **Body:** `CreateReportDto`  
* {  
*   "reportReason": 1,  
*   "additionalComment": "string"  
*   
* } but same response for post/comment  
* **Response:** `ApiResponse<bool`  
* {  
*     "success": **true**,  
*     "message": "Report submitted successfully",  
*     "data": **true**,  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`GET /api/communities/posts/{postId}/reports`**

* **Description:** Get all pending reports for a post (Owner or Admin only)  
* **Auth:** Required  
* **Params:** `postId` (Guid)  
* **Response:** `ApiResponse<IEnumerable<ReportResponseDto`  
* {  
*     "success": **true**,  
*     "message": "Success",  
*     "data": \[  
*         {  
*             "communityReportId": "292f2fee-2e6e-4eb7-be48-093e4a158ecb",  
*             "reportReason": 1,  
*             "additionalComment": **null**,  
*             "status": 1,  
*             "createdAt": "2026-05-12T22:09:28.0871536",  
*             "reporterUserId": "0cc81526-226b-4136-95e1-5dcd964bbc68",  
*             "reporterName": "Rehab Baajar",  
*             "targetPostId": "8b772237-ae10-4efa-bc88-e487c31aeb71",  
*             "targetCommentId": "00b62602-1349-46de-81d1-08deb06fcea4",  
*             "reportsCount": 2  
*         },  
*         {  
*             "communityReportId": "7bd58435-266b-46a8-a22c-73d107dd6e27",  
*             "reportReason": 1,  
*             "additionalComment": **null**,  
*             "status": 1,  
*             "createdAt": "2026-05-12T22:05:56.4034473",  
*             "reporterUserId": "0cc81526-226b-4136-95e1-5dcd964bbc68",  
*             "reporterName": "Rehab Baajar",  
*             "targetPostId": "8b772237-ae10-4efa-bc88-e487c31aeb71",  
*             "targetCommentId": **null**,  
*             "reportsCount": 2  
*         }  
*     \],  
*     "errors": **null**  
* }  
* `>>`  
* **Status Codes:** `200 OK`

#### **`PATCH /api/communities/posts/reports/{reportId}/status`**

* **Description:** Update the status of a report (Owner or Admin only)  
* **Auth:** Required  
* **Params:** `reportId` (Guid)  
* **Body:** `CommunityReportStatus` (raw enum int)  
* **Response:** `ApiResponse<bool`  
* {  
*     "success": **true**,  
*     "message": "Report status updated successfully",  
*     "data": **true**,  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

#### **`DELETE /api/communities/posts/reports/{reportId}`**

* **Description:** Delete a report (Owner or Admin only)  
* **Auth:** Required  
* **Params:** `reportId` (Guid)  
* **Response:** `ApiResponse<bool`  
* {  
*     "success": **true**,  
*     "message": "Report deleted successfully",  
*     "data": **true**,  
*     "errors": **null**  
* }  
* `>`  
* **Status Codes:** `200 OK`, `400 Bad Request`

`Get`/api/Explore/communities 

`view all communities exist in platform` 

* **Params:QuerySearch**  
* **Response:**{  
*     "success": **true**,  
*     "message": "Success",  
*     "data": \[  
*         {  
*             "communityId": "9e11fc3a-21f1-4bb7-9f08-cfd5bbdb8755",  
*             "name": "back com",  
*             "description": "A community for developers",  
*             "coverImageUrl": "https://img.com/cover.png",  
*             "creatorName": "Rehab Baajar",  
*             "domainId": 1,  
*             "membersCount": 1,  
*             "postsCount": 2  
*         },  
*         {  
*             "communityId": "ca68a38e-e231-4119-b311-e695b69d508b",  
*             "name": "Tech Hub",  
*             "description": "A community for developers",  
*             "coverImageUrl": "https://img.com/cover.png",  
*             "creatorName": "Rehab Baajar",  
*             "domainId": 1,  
*             "membersCount": 1,  
*             "postsCount": 0  
*         }  
*     \],  
*     "errors": **null**  
* }  
* `200 OK`,  
* 

---

## **6\. Uploads**

* Community cover image is stored as a plain string URL in `CoverImageUrl`.  
* There is **no** community-specific upload endpoint inside the Community module.  
* The codebase includes general file upload endpoints under `api/File`, but those are separate from the Community controller.  
* **Frontend note:** Upload the image first via `api/File`, get back the URL, then pass it as `CoverImageUrl` in `CreateCommunityDto` or `UpdateCommunityDto`.

---

## **7\. Error Responses**

All community endpoints return a consistent `ApiResponse<T>` wrapper on failure:

{  
  "success": false,  
  "message": "Error message here",  
  "data": null,  
  "errors": \["optional validation details"\]  
}

### **Common Error Messages**

| Scenario | Message |
| ----- | ----- |
| Duplicate community name | `Community name already exists` |
| Community not found | `Community not found` |
| Non-owner/admin updates community | `Only Owner or Admin can update the community` |
| Non-owner deletes community | `Only the Owner can delete the community` |
| Non-member creates post | `Only members can create posts` |
| Post not found | `Post not found` |
| Non-author updates post | `Only the post author can update` |
| No permission to delete post | `You do not have permission to delete this post` |
| Comment not found | `Comment not found` |
| Non-author updates comment | `Only the comment author can update` |
| No permission to delete comment | `You do not have permission to delete this comment` |
| Banned user tries to join | `You are banned from this community` |
| Already a member | `You are already a member of this community` |
| Not a member | `You are not a member of this community` |
| Owner tries to leave | `Owner cannot leave the community` |
| Non-owner/admin removes member | `Only Owner or Admin can remove members` |
| Target not a member | `Target user is not a member` |
| Removing Owner | `Owner cannot be removed` |
| Non-owner changes role | `Only Owner can change roles` |
| Assigning Owner role | `Cannot assign Owner role` |
| Changing Owner's role | `Cannot change Owner's role` |
| Non-owner/admin bans member | `Only Owner or Admin can ban members` |
| Banning Owner | `Owner cannot be banned` |
| Non-owner/admin updates report | `Only Owner or Admin can update report status` |
| No permission to delete report | `You do not have permission to delete this report` |

Authentication failures (missing/invalid token) return standard ASP.NET Core `401 Unauthorized` before reaching the community controllers.

---

## **8\. Frontend Notes**

All community endpoints require a valid JWT bearer token in the header:  
 Authorization: Bearer \<token\>

*   
* Use `GET /api/communities/all` for browsing all communities.  
* Use `GET /api/communities/my` to show the user's joined communities.  
* Community responses include **counts only** (`MembersCount`, `PostsCount`) — not arrays. Fetch members/posts via separate endpoints.  
* Like and Save are **toggle** endpoints — call the same endpoint to like and unlike.  
* `SharePost` returns a plain URL string, not a DTO object.  
* `CreateComment` returns only the new comment's `Guid`, not the full comment object.  
* `CreateReportDto.TargetCommentId` is optional — only include it when reporting a specific comment inside a post.  
* For role assignment, send the raw enum integer in the request body:  
  * `2` → Admin  
  * `3` → Member  
  * (Do NOT send `1` / Owner — it will be rejected)  
* `CoverImageUrl` and `AuthorProfilePicture` can be `null` — always handle missing images gracefully in the UI.  
* Report list only returns **pending** reports — resolved/closed reports will not appear.  
* No pagination exists — frontend should be prepared to handle potentially large lists.

### **UI Conditional Logic**

| Condition | UI Behavior |
| ----- | ----- |
| `currentUser.role === 1` (Owner) | Show: Edit community, Delete community, Change roles |
| `currentUser.role <= 2` (Owner or Admin) | Show: Remove member, Ban member, Delete any post/comment, Manage reports |
| `currentUser.id === post.authorId` | Show: Edit post button |
| `currentUser.id === post.authorId` OR `role <= 2` | Show: Delete post button |
| `currentUser.id === comment.authorId` | Show: Edit comment button |
| `currentUser.id === comment.authorId` OR `role <= 2` | Show: Delete comment button |
| User is not a member | Hide: Create post button |

---

## **9\. Testing Scenarios**

### **Happy Paths**

| Scenario | Expected Result |
| ----- | ----- |
| Create community | Returns `CommunityResponseDto` with correct data |
| Join community | Returns `true`, user is now a Member |
| Owner updates community | Returns `true`, details updated |
| Member creates a post | Returns `PostResponseDto` |
| Author updates their own post | Returns updated `PostResponseDto` |
| Author deletes their own post | Returns `true` |
| Admin deletes another user's post | Returns `true` |
| Create comment on a post | Returns new comment `Guid` |
| Author updates comment | Returns updated `CommentResponseDto` |
| Author deletes comment | Returns `true` |
| Like a post | Returns `true` (liked) |
| Like same post again | Returns `true` (unliked — toggle) |
| Save a post | Returns `true` (saved) |
| Save same post again | Returns `true` (unsaved — toggle) |
| Share a post | Returns shareable URL string |
| Report a post | Returns `true` |
| Owner/Admin update report status | Returns `true` |

### **Failure Paths**

| Scenario | Expected Error |
| ----- | ----- |
| Create community with duplicate name | `Community name already exists` |
| Non-member tries to create a post | `Only members can create posts` |
| User updates another user's post | `Only the post author can update` |
| Member tries to update community | `Only Owner or Admin can update the community` |
| Owner tries to leave community | `Owner cannot leave the community` |
| Attempt to ban the Owner | `Owner cannot be banned` |
| Attempt to remove the Owner | `Owner cannot be removed` |
| Assign Owner role via role update | `Cannot assign Owner role` |
| Report a non-existent comment | `400 Bad Request` |
| Member tries to delete a report | `You do not have permission to delete this report` |
| Request with no/invalid token | `401 Unauthorized` |

---

## **10\. Authentication**

* Authentication is **JWT-based**, configured in `Program.cs`.  
* All community controllers use the `[Authorize]` attribute.  
* The user ID is extracted from JWT claims using `User.GetUserId()` (defined in `ClaimsPrincipalExtensions.cs`).  
* The token must contain `ClaimTypes.NameIdentifier` with the user's GUID.  
* **No community endpoint supports anonymous access.**

**How to send the token:**

GET /api/communities/all  
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

---

## **11\. Response Consistency**

All community endpoints wrap their response in `ApiResponse<T>`:

### **Success Response**

{  
  "success": true,  
  "message": "Optional descriptive message",  
  "data": { ... },  
  "errors": null  
}

### **Failure Response**

{  
  "success": false,  
  "message": "Error reason here",  
  "data": null,  
  "errors": \["optional validation details"\]  
}

### **Frontend Handling Pattern**

const response \= await api.post('/communities', body);

if (response.data.success) {  
  // use response.data.data  
} else {  
  // show response.data.message  
}

* Controllers return `200 OK` when `result.Success == true`  
* Controllers return `400 Bad Request` when `result.Success == false`  
* Always check `success` first, then read `data` or `message` accordingly.

